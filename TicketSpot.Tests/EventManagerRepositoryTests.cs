using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class EventManagerRepositoryTests
{
    private readonly IEventManagerRepository _eventManagerRepository
        = new EventManagerRepository(new DataAccessLayer.TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _eventManagerRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task EventManager_Create_GetById()
    {
        var eventManagerToCreate = new EventManager
        {
            FirstName = "Bill",
            LastName = "Gates",
            Email = "b.gates@microsoft.com"
        };

        await _eventManagerRepository.CreateAsync(eventManagerToCreate);

        var resultGetAll = await _eventManagerRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(2);

        var createdId = resultGetAll.First(em => em.LastName == "Gates").Id;

        cleanupId = createdId;

        var resultGetById = await _eventManagerRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(eventManagerToCreate);
    }

    [TestMethod]
    public async Task EventManager_Update()
    {
        var eventManagerToCreate = new EventManager
        {
            FirstName = "Bill",
            LastName = "Gates",
            Email = "b.gates@microsoft.com"
        };

        await _eventManagerRepository.CreateAsync(eventManagerToCreate);

        var resultGetAll = await _eventManagerRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(2);

        var createdId = resultGetAll.First(em => em.LastName == "Gates").Id;

        cleanupId = createdId;

        var eventManagerToUpdate = new EventManager
        {
            Id = createdId,
            FirstName = "Jill",
            LastName = "Gates",
            Email = "b.gates@microsoft.com"
        };

        var updateResult = await _eventManagerRepository.UpdateAsync(eventManagerToUpdate);

        var resultGetById = await _eventManagerRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(eventManagerToUpdate);
    }
}
