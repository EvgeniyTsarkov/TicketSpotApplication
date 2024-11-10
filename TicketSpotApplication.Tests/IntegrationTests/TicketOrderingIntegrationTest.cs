﻿using Common.Models;
using Common.Models.Enums;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PublicWebAPI.Business.Dtos;
using System.Net;
using TicketSpotApplication.Tests.Helpers;

namespace TicketSpotApplication.Tests.IntegrationTests;

[TestClass]
public class TicketOrderingIntegrationTest
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private IServiceScope _scope;
    private ICartRepository _cartRepository;
    private IRepository<Ticket> _ticketRepository;
    private IRepository<Payment> _paymentRepository;
    private IRepository<Customer> _customerRepository;
    private IRepository<PriceOption> _priceOptionRepository;

    private static readonly Guid _cartId = Guid.NewGuid();

    private readonly Customer _customer = new()
    {
        FirstName = "Maximilian",
        LastName = "Hawthorne",
        Email = "m.hawthorne@1.com"
    };

    private readonly Payment _payment = new()
    {
        Status = PaymentStatus.Disputed,
        TotalAmount = 0
    };

    private readonly PriceOption _priceOption = new()
    {
        Name = "TestPrice",
        Price = 100
    };

    private readonly Ticket _ticket = new()
    {
        CartId = null,
        CustomerId = null,
        EventId = 1,
        SeatId = 1,
        PriceOptionId = 0,
    };

    private readonly Cart _cart = new()
    {
        Id = _cartId,
        CartStatus = CartStatus.Empty,
        CustomerId = 0,
        PaymentId = 0,
    };


    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<TicketSpotDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TicketSpotDb;Trusted_Connection=True;"));

                services.AddScoped<ICartRepository, CartRepository>();
                services.AddScoped<IRepository<Ticket>, GenericRepository<Ticket>>();
                services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
                services.AddScoped<IRepository<Customer>, GenericRepository<Customer>>();
                services.AddScoped<IRepository<PriceOption>, GenericRepository<PriceOption>>();
            });
        });

        _client = _factory.CreateClient();

        _scope = _factory.Services.CreateScope();

        _cartRepository = _scope.ServiceProvider.GetRequiredService<ICartRepository>();
        _ticketRepository = _scope.ServiceProvider.GetRequiredService<IRepository<Ticket>>();
        _paymentRepository = _scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();
        _customerRepository = _scope.ServiceProvider.GetRequiredService<IRepository<Customer>>();
        _priceOptionRepository = _scope.ServiceProvider.GetRequiredService<IRepository<PriceOption>>();
    }

    [TestMethod]
    public async Task OrdersShallBePlacedAndReleasedCorrectly()
    {
        // Arrange
        await SeedTestData();

        // Act - Place Tickets to Cart
        var orderPayload = new OrderPayloadDto
        {
            EventId = 1,
            SeatId = 1,
            PriceOptionId = 1
        };

        var httpContent = TestHelper.BuildHttpContent(orderPayload);

        var placeOrderResponse = await _client.PostAsync($"/orders/carts/{_cartId}", httpContent);

        // Assert - Place Tickets to Cart
        placeOrderResponse.Should().NotBeNull();
        placeOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Wrong response status code.");

        var cartWithTickets = await _cartRepository.GetAsync(_cartId);

        cartWithTickets.Should().NotBeNull("Cart does not exist");
        cartWithTickets.Tickets.Should().NotBeNullOrEmpty("Tickets have not been added to the cart.");
        cartWithTickets.Tickets.Count.Should().Be(1, "The number of tickets in the cart is incorrect.");

        var addedTicket = cartWithTickets.Tickets.First();
        addedTicket.TicketStatus.Should().Be(TicketStatus.Booked, "Ticket status is incorrect.");
        addedTicket.CartId.Should().Be(_cartId, "Cart id in the ticket is wrong.");
        addedTicket.CustomerId.Should().Be(_cart.CustomerId, "Customer id in the ticket is wrong.");

        cartWithTickets.Payment.Status.Should().Be(PaymentStatus.Pending, "Payment status is wrong");
        cartWithTickets.Payment.TotalAmount.Should().Be(addedTicket.PriceOption.Price, "Payment total amount is incorrect.");

        cartWithTickets.CartStatus.Should().Be(CartStatus.Active, "Cart status is incorrect.");

        // Act - Release tickets
        var releaseOrderResponse = await _client.DeleteAsync($"/orders/carts/{_cartId}/events/{orderPayload.EventId}/seats/{orderPayload.SeatId}");

        // Assert - Release tickets
        releaseOrderResponse.Should().NotBeNull();
        releaseOrderResponse.StatusCode.Should().Be(HttpStatusCode.NoContent, "Wrong response status code.");

        var cartWithReleasedTickets = await _cartRepository.GetAsync(_cartId);

        cartWithReleasedTickets.Should().NotBeNull("Cart does not exist.");
        cartWithReleasedTickets.Tickets.Should().BeEmpty("Tickets habe not been deleted.");

        var removedTicket = await _ticketRepository.GetAsync(1);
        removedTicket.Should().NotBeNull("The requested ticket does not exist.");
        removedTicket.CustomerId.Should().BeNull("Customer id has not been reset after ticket release.");
        removedTicket.TicketStatus.Should().Be(TicketStatus.Available, "Ticket status has not been reset after ticket release.");
        removedTicket.CartId.Should().BeNull("Cart id has not been resent after ticket release.\"");

        var paymentAfterRemoval = await _paymentRepository.GetAsync(_cart.PaymentId);
        paymentAfterRemoval.Should().NotBeNull("The requested payment does not exist.");
        paymentAfterRemoval.Status.Should().Be(PaymentStatus.Cancelled, "New payment status has not been assigned.");
        paymentAfterRemoval.TotalAmount.Should().Be(0);

        cartWithReleasedTickets.CartStatus.Should().Be(CartStatus.Empty, "Cart status is incorrect.");
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await RemoveTestDataAsync();

        _client.Dispose();
        _factory.Dispose();
    }

    private async Task SeedTestData()
    {
        // Seed customer
        var existingCustomer = await _customerRepository.GetByConditionAsync(c =>
        c.FirstName == _customer.FirstName
        && c.LastName == _customer.LastName);

        if (existingCustomer == null)
        {
            await _customerRepository.CreateAsync(_customer);
            existingCustomer = await _customerRepository.GetByConditionAsync(c =>
                c.FirstName == _customer.FirstName
                && c.LastName == _customer.LastName);
        }

        existingCustomer.Should().NotBeNull();

        _customer.Id = existingCustomer.Id;


        // Seed payment
        var existingPayment = await _paymentRepository.GetByConditionAsync(p =>
        p.Status == _payment.Status
        && p.TotalAmount == _payment.TotalAmount);

        if (existingPayment == null)
        {
            await _paymentRepository.CreateAsync(_payment);
            existingPayment = await _paymentRepository.GetByConditionAsync(p =>
            p.Status == _payment.Status
            && p.TotalAmount == _payment.TotalAmount);
        }

        existingPayment.Should().NotBeNull();

        _payment.Id = existingPayment.Id;


        // Seed price option
        var existingPriceOption = await _priceOptionRepository.GetByConditionAsync(po =>
        po.Name == _priceOption.Name
        && po.Price == _priceOption.Price);

        if (existingPriceOption == null)
        {
            await _priceOptionRepository.CreateAsync(_priceOption);
            existingPriceOption = await _priceOptionRepository.GetByConditionAsync(po =>
            po.Name == _priceOption.Name
            && po.Price == _priceOption.Price);
        }

        existingPriceOption.Should().NotBeNull();

        _priceOption.Id = existingPriceOption.Id;


        // Seed ticket
        var existingTicket = await _ticketRepository.GetByConditionAsync(t =>
        t.SeatId == _ticket.SeatId
        && t.EventId == _ticket.EventId
        && t.PriceOptionId == _priceOption.Id);

        if (existingTicket == null)
        {
            _ticket.PriceOptionId = _priceOption.Id;
            await _ticketRepository.CreateAsync(_ticket);
            existingTicket = await _ticketRepository.GetByConditionAsync(t =>
            t.SeatId == _ticket.SeatId
            && t.EventId == _ticket.EventId
            && t.PriceOptionId == _priceOption.Id);
        }

        existingTicket.Should().NotBeNull();

        _ticket.Id = existingTicket.Id;


        // Seed cart
        _cart.CustomerId = existingCustomer.Id;
        _cart.PaymentId = existingPayment.Id;

        await _cartRepository.CreateAsync(_cart);

        var createdCart = await _cartRepository.GetAsync(_cartId);

        createdCart.Should().NotBeNull();
    }

    private async Task RemoveTestDataAsync()
    {
        await _customerRepository.DeleteAsync(_customer.Id);

        await _paymentRepository.DeleteAsync(_payment.Id);

        await _priceOptionRepository.DeleteAsync(_priceOption.Id);
    }
}
