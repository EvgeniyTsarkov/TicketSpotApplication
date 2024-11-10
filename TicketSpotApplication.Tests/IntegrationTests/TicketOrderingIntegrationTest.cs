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
            });
        });

        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task OrdersShallBePlacedAndReleasedCorrectly()
    {
        // Arrange
        var cart = new Cart
        {
            Id = _cartId,
            CartStatus = CartStatus.Empty,
            CustomerId = 1,
            PaymentId = 1,
        };

        using var scope = _factory.Services.CreateScope();

        var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

        await cartRepository.CreateAsync(cart);

        var createdCart = await cartRepository.GetAsync(cart.Id);

        createdCart.Should().NotBeNull("Cart has not been created in the database.");
        createdCart.Should().BeEquivalentTo(cart, o => o
        .Excluding(f => f.Payment)
        .Excluding(f => f.Customer),
        "The created cart has not match the expected one.");


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
        using var scope = _factory.Services.CreateScope();

        var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

        await cartRepository.DeleteAsync(_cartId);

        var ticketToRevert = new Ticket
        {
            Id = 1,
            TicketStatus = TicketStatus.Available,
            CartId = null,
            EventId = 1,
            SeatId = 1,
            PriceOptionId = 1,
        };

        var ticketRepository = scope.ServiceProvider.GetRequiredService<IRepository<Ticket>>();

        await ticketRepository.UpdateAsync(ticketToRevert);

        var paymentToRevert = new Payment
        {
            Id = 1,
            Status = PaymentStatus.Pending,
            TotalAmount = 0,
        };

        var paymentRepository = scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();

        await paymentRepository.UpdateAsync(paymentToRevert);

        _client.Dispose();
        _factory.Dispose();
    }
}
