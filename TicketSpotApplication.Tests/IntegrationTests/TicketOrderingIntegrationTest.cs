using Common.Models;
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

    private readonly Guid _cartId = Guid.NewGuid();

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
    }

    [TestMethod]
    public async Task OrdersShallBePlacedAndReleasedCorrectly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        await SeedTestData();

        // Seed cart
        var cart = new Cart
        {
            Id = _cartId,
            CartStatus = CartStatus.Empty,
            CustomerId = _customer.Id,
            PaymentId = _payment.Id,
        };

        var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

        await cartRepository.CreateAsync(cart);

        var createdCart = await cartRepository.GetAsync(cart.Id);

        createdCart.Should().NotBeNull();


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

        var cartWithTickets = await cartRepository.GetAsync(cart.Id);

        cartWithTickets.Should().NotBeNull("Cart does not exist");
        cartWithTickets.Tickets.Should().NotBeNullOrEmpty("Tickets have not been added to the cart.");
        cartWithTickets.Tickets.Count.Should().Be(1, "The number of tickets in the cart is incorrect.");

        var addedTicket = cartWithTickets.Tickets.First();
        addedTicket.TicketStatus.Should().Be(TicketStatus.Booked, "Ticket status is incorrect.");
        addedTicket.CartId.Should().Be(_cartId, "Cart id in the ticket is wrong.");
        addedTicket.CustomerId.Should().Be(cart.CustomerId, "Customer id in the ticket is wrong.");

        cartWithTickets.Payment.Status.Should().Be(PaymentStatus.Pending, "Payment status is wrong");
        cartWithTickets.Payment.TotalAmount.Should().Be(addedTicket.PriceOption.Price, "Payment total amount is incorrect.");

        cartWithTickets.CartStatus.Should().Be(CartStatus.Active, "Cart status is incorrect.");


        // Act - Release tickets
        var releaseOrderResponse = await _client.DeleteAsync($"/orders/carts/{_cartId}/events/{orderPayload.EventId}/seats/{orderPayload.SeatId}");

        // Assert - Release tickets
        releaseOrderResponse.Should().NotBeNull();
        releaseOrderResponse.StatusCode.Should().Be(HttpStatusCode.NoContent, "Wrong response status code.");

        var cartWithReleasedTickets = await cartRepository.GetAsync(_cartId);

        cartWithReleasedTickets.Should().NotBeNull("Cart does not exist.");
        cartWithReleasedTickets.Tickets.Should().BeEmpty("Tickets habe not been deleted.");

        var ticketRepository = scope.ServiceProvider.GetRequiredService<IRepository<Ticket>>();
        var removedTicket = await ticketRepository.GetAsync(1);

        removedTicket.Should().NotBeNull("The requested ticket does not exist.");
        removedTicket.CustomerId.Should().BeNull("Customer id has not been reset after ticket release.");
        removedTicket.TicketStatus.Should().Be(TicketStatus.Available, "Ticket status has not been reset after ticket release.");
        removedTicket.CartId.Should().BeNull("Cart id has not been resent after ticket release.\"");

        var paymentRepository = scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();
        var paymentAfterRemoval = await paymentRepository.GetAsync(cart.PaymentId);
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
        using var scope = _factory.Services.CreateScope();

        // Seed customer
        var customerRepository = scope.ServiceProvider.GetRequiredService<IRepository<Customer>>();

        var existingCustomer = await customerRepository.GetByConditionAsync(c =>
        c.FirstName == _customer.FirstName
        && c.LastName == _customer.LastName);

        if (existingCustomer == null)
        {
            await customerRepository.CreateAsync(_customer);
            existingCustomer = await customerRepository.GetByConditionAsync(c =>
                c.FirstName == _customer.FirstName
                && c.LastName == _customer.LastName);
        }

        existingCustomer.Should().NotBeNull();

        _customer.Id = existingCustomer.Id;


        // Seed payment
        var paymentRepository = scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();

        var existingPayment = await paymentRepository.GetByConditionAsync(p =>
        p.Status == _payment.Status
        && p.TotalAmount == _payment.TotalAmount);

        if (existingPayment == null)
        {
            await paymentRepository.CreateAsync(_payment);
            existingPayment = await paymentRepository.GetByConditionAsync(p =>
            p.Status == _payment.Status
            && p.TotalAmount == _payment.TotalAmount);
        }

        existingPayment.Should().NotBeNull();

        _payment.Id = existingPayment.Id;


        // Seed price option
        var priceOptionRepository = scope.ServiceProvider.GetRequiredService<IRepository<PriceOption>>();

        var existingPriceOption = await priceOptionRepository.GetByConditionAsync(po =>
        po.Name == _priceOption.Name
        && po.Price == _priceOption.Price);

        if (existingPriceOption == null)
        {
            await priceOptionRepository.CreateAsync(_priceOption);
            existingPriceOption = await priceOptionRepository.GetByConditionAsync(po =>
            po.Name == _priceOption.Name
            && po.Price == _priceOption.Price);
        }

        existingPriceOption.Should().NotBeNull();

        _priceOption.Id = existingPriceOption.Id;


        // Seed ticket
        var ticketRepository = scope.ServiceProvider.GetRequiredService<IRepository<Ticket>>();

        var existingTicket = await ticketRepository.GetByConditionAsync(t =>
        t.SeatId == _ticket.SeatId
        && t.EventId == _ticket.EventId
        && t.PriceOptionId == _priceOption.Id);

        if (existingTicket == null)
        {
            _ticket.PriceOptionId = _priceOption.Id;
            await ticketRepository.CreateAsync(_ticket);
            existingTicket = await ticketRepository.GetByConditionAsync(t =>
            t.SeatId == _ticket.SeatId
            && t.EventId == _ticket.EventId
            && t.PriceOptionId == _priceOption.Id);
        }

        existingTicket.Should().NotBeNull();

        _ticket.Id = existingTicket.Id;
    }

    private async Task RemoveTestDataAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var customerRepository = scope.ServiceProvider.GetRequiredService<IRepository<Customer>>();

        await customerRepository.DeleteAsync(_customer.Id);

        var paymentRepository = scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();

        await paymentRepository.DeleteAsync(_payment.Id);

        var priceOptionRepository = scope.ServiceProvider.GetRequiredService<IRepository<PriceOption>>();

        await priceOptionRepository.DeleteAsync(_priceOption.Id);
    }
}
