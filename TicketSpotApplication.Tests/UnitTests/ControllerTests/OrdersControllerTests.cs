using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.ExceptionExtensions;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;
using PublicWebAPI.Controllers;

namespace TicketSpotApplication.Tests.UnitTests.ControllerTests;

[TestClass]
public class OrdersControllerTests
{
    private readonly IOrderService _orderService = Substitute.For<IOrderService>();
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _controller = new OrdersController(_orderService);
    }

    [TestMethod]
    public async Task GetTicketsByCartId_ReturnsOkResult_WithListOfTickets()
    {
        // Arrange
        _orderService.GetTicketsByCartIdAsync(Arg.Any<string>()).Returns([new(), new()]);

        // Act 
        var result = await _controller.GetTicketsByCartId(new Guid().ToString());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<Ticket>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }

    [TestMethod]
    public async Task AddTicketToCart_ReturnsOkResult_CartStatus()
    {
        // Arrange
        _orderService.AddTicketsToCartAsync(Arg.Any<string>(), Arg.Any<OrderPayloadDto>()).Returns(CartStatus.Active);

        // Act
        var result = await _controller.AddTicketToCart(new Guid().ToString(), new OrderPayloadDto());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = (CartStatus)okResult.Value;
        returnValue.Should().Be(CartStatus.Active);
    }

    [TestMethod]
    public async Task AddTicketToCart_ReturnsBadRequestResult()
    {
        // Arrange
        _orderService.AddTicketsToCartAsync(Arg.Any<string>(), Arg.Any<OrderPayloadDto>())
            .Throws(new RecordNotFoundException("Test"));

        // Act
        var result = await _controller.AddTicketToCart(new Guid().ToString(), new OrderPayloadDto());

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public async Task DeleteTicketFromCart_ReturnsNoContent()
    {
        // Act 
        var result = await _controller.DeleteTicketFromCart(string.Empty, 0, 0);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task DeleteTicketFromCart_ReturnsBadRequestResult()
    {
        // Arrange
        _orderService.DeleteSeatFromCartAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>())
            .Throws(new RecordNotFoundException("Test"));

        // Act 
        var result = await _controller.DeleteTicketFromCart(string.Empty, 0, 0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public async Task ChangeTicketsStatusToBooked_ReturnsOkResult_WithListOfTickets()
    {
        // Arrange
        _orderService.ChangeStatusOfAllTicketsInCartToBooked(Arg.Any<string>()).Returns([new(), new()]);

        // Act 
        var result = await _controller.ChangeTicketsStatusToBooked(new Guid().ToString());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<Ticket>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }

    [TestMethod]
    public async Task ChangeTicketsStatusToBooked_ReturnsBadRequestResult()
    {
        // Arrange
        _orderService.ChangeStatusOfAllTicketsInCartToBooked(Arg.Any<string>())
            .Throws(new RecordNotFoundException("Test"));

        // Act 
        var result = await _controller.ChangeTicketsStatusToBooked(string.Empty);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
