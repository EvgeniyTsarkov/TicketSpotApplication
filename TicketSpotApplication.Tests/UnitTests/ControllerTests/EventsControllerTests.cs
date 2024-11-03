using Common.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PublicWebAPI.Business.Services.Interfaces;
using PublicWebAPI.Controllers;

namespace TicketSpotApplication.Tests.UnitTests.ControllerTests;

[TestClass]
public class EventsControllerTests
{
    private IEventService _eventService = Substitute.For<IEventService>();

    [TestMethod]
    public async Task GetAll_ReturnsOkResult_WithListOfEvents()
    {
        // Arrange
        _eventService.GetAllAsync().Returns([new(), new()]);

        var controller = new EventsController(_eventService);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<Event>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }
}
