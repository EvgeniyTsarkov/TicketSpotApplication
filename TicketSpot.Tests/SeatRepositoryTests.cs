using Common.Models;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class SeatRepositoryTests
{
    private readonly ISeatRepository _seatRepository = new SeatRepository(new TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _seatRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task Seat_Create_GetById()
    {
        var seatToCreate = new Seat
        {
            Section = "B",
            RowNumber = 5,
            SeatNumber = 5,
            VenueId = 1
        };

        await _seatRepository.CreateAsync(seatToCreate);

        var resultGetAll = await _seatRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(s => s.Section == "B").Id;

        cleanupId = createdId;

        var resultGetById = await _seatRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(seatToCreate);
    }

    [TestMethod]
    public async Task Event_Update()
    {
        var seatToCreate = new Seat
        {
            Section = "B",
            RowNumber = 5,
            SeatNumber = 5,
            VenueId = 1
        };

        await _seatRepository.CreateAsync(seatToCreate);

        var resultGetAll = await _seatRepository.GetAllAsync();

        resultGetAll.Should().HaveCountGreaterThan(1);

        var createdId = resultGetAll.First(s => s.Section == "B").Id;

        cleanupId = createdId;

        var seatToUpdate = new Seat
        {
            Id = createdId,
            Section = "B",
            RowNumber = 5,
            SeatNumber = 5,
            VenueId = 1
        };

        var updateResult = await _seatRepository.UpdateAsync(seatToUpdate);

        var resultGetById = await _seatRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(seatToUpdate);
    }
}
