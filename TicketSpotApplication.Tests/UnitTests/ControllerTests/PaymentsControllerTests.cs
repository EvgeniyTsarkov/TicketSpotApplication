using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;
using PublicWebAPI.Controllers;

namespace TicketSpotApplication.Tests.UnitTests.ControllerTests;

[TestClass]
public class PaymentsControllerTests
{
    private readonly IPaymentService _paymentService = Substitute.For<IPaymentService>();
    private readonly PaymentsController _controller;

    public PaymentsControllerTests()
    {
        _controller = new PaymentsController(_paymentService);
    }

    [TestMethod]
    public async Task GetPaymentStatus_ReturnsOkResult_WithPaymentStatusMessage()
    {
        // Arrange
        _paymentService.GetPaymentStatusAsync(Arg.Any<int>()).Returns(PaymentStatus.InProgress);

        // Act
        var result = await _controller.GetPaymentStatus(0);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull().And.Be($"Payment Status: {PaymentStatus.InProgress}");
    }

    [TestMethod]
    public async Task GetPaymentStatus_ReturnsBadRequestResult()
    {
        // Arrange
        _paymentService.GetPaymentStatusAsync(Arg.Any<int>()).Throws(new RecordNotFoundException("Test"));

        // Act
        var result = await _controller.GetPaymentStatus(0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public async Task UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold_ReturnsOkResult_WithSeatsToPaymentObject()
    {
        // Arrange
        _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(Arg.Any<int>(), Arg.Any<PaymentStatus>(), Arg.Any<TicketStatus>())
            .Returns(new SeatsToPaymentDto());

        // Act
        var result = await _controller.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(0);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as SeatsToPaymentDto;
        returnValue.Should().BeEquivalentTo(new SeatsToPaymentDto());
    }

    [TestMethod]
    public async Task UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold_ReturnsBadRequestResult()
    {
        // Arrange
        _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(Arg.Any<int>(), Arg.Any<PaymentStatus>(), Arg.Any<TicketStatus>())
            .Throws(new RecordNotFoundException("Test"));

        // Act
        var result = await _controller.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public async Task UpdatePaymentStatusAndMarkAllRelatedSeatsAsAvailable_ReturnsOkResult_WithSeatsToPaymentObject()
    {
        // Arrange
        _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(Arg.Any<int>(), Arg.Any<PaymentStatus>(), Arg.Any<TicketStatus>())
            .Returns(new SeatsToPaymentDto());

        // Act
        var result = await _controller.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(0);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as SeatsToPaymentDto;
        returnValue.Should().BeEquivalentTo(new SeatsToPaymentDto());
    }

    [TestMethod]
    public async Task UpdatePaymentStatusAndMarkAllRelatedSeatsAsAvailable_ReturnsBadRequestResult()
    {
        // Arrange
        _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(Arg.Any<int>(), Arg.Any<PaymentStatus>(), Arg.Any<TicketStatus>())
            .Throws(new RecordNotFoundException("Test"));

        // Act
        var result = await _controller.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
