using Common.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;
using PublicWebAPI.Controllers;

namespace TicketSpotApplication.Tests.UnitTests.ControllerTests;

[TestClass]
public class EventsControllerTests
{
    private readonly IEventService _eventService = Substitute.For<IEventService>();
    private readonly EventsController _controller;

    public EventsControllerTests() => _controller = new EventsController(_eventService);

    [TestMethod]
    public async Task GetAll_ReturnsOkResult_WithListOfEvents()
    {
        // Arrange
        _eventService.GetAllAsync().Returns([new(), new()]);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<Event>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }

    [TestMethod]
    public async Task GetByIdAndSectionId_ReturnsOkResult_WithListOfSeats()
    {
        // Arrange
        _eventService.GetByIdAndSectionId(Arg.Any<int>(), Arg.Any<int>()).Returns([new(), new()]);

        // Act
        var result = await _controller.GetByIdAndSectionId(1, 1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<SeatWithPricesDto>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }
}
