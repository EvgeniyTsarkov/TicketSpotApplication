using Common.Models;
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
public class VenuesControllerTests
{
    private readonly IVenueService _venueService = Substitute.For<IVenueService>();
    private readonly VenuesController _controller;

    public VenuesControllerTests()
    {
        _controller = new VenuesController(_venueService);
    }

    [TestMethod]
    public async Task GetAll_ReturnsOkResult_WithListOfVenues()
    {
        // Arrange
        _venueService.GetAllAsync().Returns([new(), new()]);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as List<Venue>;
        returnValue.Should().NotBeNull().And.HaveCount(2);
    }

    [TestMethod]
    public async Task GetSections_ReturnsOkResult_WithSectionsToVelueDto()
    {
        // Arrange
        _venueService.GetSectionsForVenueAsync(Arg.Any<int>()).Returns(new SectionsToVenueDto());

        // Act
        var result = await _controller.GetSections(0);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
        var returnValue = okResult.Value as SectionsToVenueDto;
        returnValue.Should().BeEquivalentTo(new SectionsToVenueDto());
    }

    [TestMethod]
    public async Task GetSections_ReturnsBadRequestResult()
    {
        // Arrange
        _venueService.GetSectionsForVenueAsync(Arg.Any<int>())
            .Throws(new RecordNotFoundException("Test"));

        // Act
        var result = await _controller.GetSections(0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
