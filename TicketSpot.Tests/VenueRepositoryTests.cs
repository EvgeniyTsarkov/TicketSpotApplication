using Common.Models;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class VenueRepositoryTests
{
    private readonly IVenueRepository _venueRepository = new VenueRepository(new TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _venueRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task Venue_Create_GetById()
    {
        var venueToCreate = new Venue
        {
            Name = "Staples Center",
            Address = "LA",
            Description = "Home of the Lakers",
            EventManagerId = 1
        };

        await _venueRepository.CreateAsync(venueToCreate);

        var resultGetAll = await _venueRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(s => s.Description.Contains("Lakers")).Id;

        cleanupId = createdId;

        var resultGetById = await _venueRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(venueToCreate);
    }

    [TestMethod]
    public async Task Venue_Update()
    {
        var venueToCreate = new Venue
        {
            Name = "Staples Center",
            Address = "LA",
            Description = "Home of the Lakers",
            EventManagerId = 1
        };

        await _venueRepository.CreateAsync(venueToCreate);

        var resultGetAll = await _venueRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(s => s.Description.Contains("Lakers")).Id;

        cleanupId = createdId;

        var venueToUpdate = new Venue
        {
            Id = createdId,
            Name = "Staples Arena",
            Address = "LA",
            Description = "Home of the Lakers",
            EventManagerId = 1
        };

        var updateResult = await _venueRepository.UpdateAsync(venueToUpdate);

        var resultGetById = await _venueRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(venueToUpdate);
    }
}
