using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class TicketRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Ticket>(ticketSpotContext), ITicketRepository
{
    public new async Task<List<Ticket>> GetAllAsync() =>
        await _context.Tickets
        .AsNoTracking()
        .AsQueryable()
        .Include(t => t.Customer)
        .Include(t => t.Seat)
        .Include(t => t.Event)
        .ToListAsync();

    public async Task<Ticket> GetAsync(int id) =>
        await _context.Tickets
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(t => t.Customer)
        .Include(t => t.Seat)
        .Include(t => t.Event)
        .SingleOrDefaultAsync();

    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        var itemToUpdate = await Get(x => x.Id == ticket.Id)
            ?? throw new RecordNotFoundException("The ticket to be updated is not found in the database");

        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Ticket with id: {0} is not found in the database", id));

        _context.Tickets.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
