using Common.Models;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class TicketRepositoryTests
{
    private readonly ITicketRepository _ticketRepository = new TicketRepository(new TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _ticketRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task Ticket_Create_GetById()
    {
        var ticketToCreate = new Ticket
        {
            Price = 1.25m,
            PurchaseDate = DateTime.UtcNow,
            EventId = 1,
            SeatId = 1,
            CustomerId = 1
        };

        await _ticketRepository.CreateAsync(ticketToCreate);

        var resultGetAll = await _ticketRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(1);

        var createdId = resultGetAll.First().Id;

        cleanupId = createdId;

        var resultGetById = await _ticketRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(ticketToCreate);
    }

    [TestMethod]
    public async Task Ticker_Update()
    {
        var ticketToCreate = new Ticket
        {
            Price = 1.25m,
            PurchaseDate = DateTime.UtcNow,
            EventId = 1,
            SeatId = 1,
            CustomerId = 1
        };

        await _ticketRepository.CreateAsync(ticketToCreate);

        var resultGetAll = await _ticketRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(1);

        var createdId = resultGetAll.First().Id;

        cleanupId = createdId;

        var ticketToUpdate = new Ticket
        {
            Id = createdId,
            Price = 2.25m,
            PurchaseDate = DateTime.UtcNow,
            EventId = 1,
            SeatId = 1,
            CustomerId = 1
        };

        var updateResult = await _ticketRepository.UpdateAsync(ticketToUpdate);

        var resultGetById = await _ticketRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(ticketToUpdate);
    }
}
