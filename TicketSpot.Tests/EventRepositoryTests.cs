using Common.Models;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class EventRepositoryTests
{
    private readonly IEventRepository _eventRepository = new EventRepository(new TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _eventRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task Event_Create_GetById()
    {
        var eventToCreate = new Event
        {
            Name = "U2 Concert",
            Date = DateTime.Parse("2024.12.25 19:00"),
            Description = "Jushua Tree album aniversary",
            EventManagerId = 1
        };

        await _eventRepository.CreateAsync(eventToCreate);

        var resultGetAll = await _eventRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(e => e.Name.StartsWith("U2")).Id;

        cleanupId = createdId;

        var resultGetById = await _eventRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(eventToCreate);
    }

    [TestMethod]
    public async Task Event_Update()
    {
        var eventToCreate = new Event
        {
            Name = "U2 Concert",
            Date = DateTime.Parse("2024.12.25 19:00"),
            Description = "Jushua Tree album aniversary",
            EventManagerId = 1
        };

        await _eventRepository.CreateAsync(eventToCreate);

        var resultGetAll = await _eventRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(e => e.Name.StartsWith("U2")).Id;

        cleanupId = createdId;

        var eventToUpdate = new Event
        {
            Id = createdId,
            Name = "U2 Concert",
            Date = DateTime.Parse("2024.12.25 20:00"),
            Description = "Jushua Tree album aniversary",
            EventManagerId = 1
        };

        var updateResult = await _eventRepository.UpdateAsync(eventToUpdate);

        var resultGetById = await _eventRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(eventToUpdate);
    }
}
