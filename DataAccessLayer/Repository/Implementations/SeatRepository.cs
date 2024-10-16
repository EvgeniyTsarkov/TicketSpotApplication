using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class SeatRepository(TicketSpotDbContext context) : IRepository<Seat>
{
    public new async Task<List<Seat>> GetAllAsync() =>
        await context.Seats
        .AsNoTracking()
        .AsQueryable()
        .ToListAsync();

    public async Task<Seat> GetAsync(int id) =>
        await context.Seats
        .AsNoTracking()
        .AsQueryable()
        .Where(s => s.Id == id)
        .SingleOrDefaultAsync();

    public async Task<Seat> CreateAsync(Seat seat)
    {
        await context.Seats.AddAsync(seat);
        await context.SaveChangesAsync();
        return seat;
    }

    public async Task<Seat> UpdateAsync(Seat seat)
    {
        var itemToUpdate = await GetAsync(seat.Id)
            ?? throw new RecordNotFoundException("The seat to be updated is not found in the database");

        context.Seats.Update(seat);
        await context.SaveChangesAsync();
        return seat;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Seat { Id = id };

        context.Attach(itemToDelete);
        context.Seats.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
