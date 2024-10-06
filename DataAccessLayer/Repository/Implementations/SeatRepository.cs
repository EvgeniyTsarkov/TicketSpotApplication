using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class SeatRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Seat>(ticketSpotContext), ISeatRepository
{
    public new async Task<List<Seat>> GetAllAsync() =>
        await _context.Seats
        .AsNoTracking()
        .AsQueryable()
        .Include(s => s.Row)
        .ToListAsync();

    public async Task<Seat> GetAsync(int id) =>
        await _context.Seats
        .AsNoTracking()
        .AsQueryable()
        .Where(s => s.Id == id)
        .Include(s => s.Row)
        .SingleOrDefaultAsync();

    public async Task<Seat> CreateAsync(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();
        return seat;
    }

    public async Task<Seat> UpdateAsync(Seat seat)
    {
        var itemToUpdate = await Get(x => x.Id == seat.Id)
            ?? throw new RecordNotFoundException("The seat to be updated is not found in the database");

        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
        return seat;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Seat with id: {0} is not found in the database", id));

        _context.Seats.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
