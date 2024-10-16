using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class TicketRepository(TicketSpotDbContext context) : IRepository<Ticket>
{
    public new async Task<List<Ticket>> GetAllAsync() =>
        await context.Tickets
        .AsNoTracking()
        .AsQueryable()
        .Include(t => t.Customer)
        .Include(t => t.Seat)
        .Include(t => t.Event)
        .ToListAsync();

    public async Task<Ticket> GetAsync(int id) =>
        await context.Tickets
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(t => t.Customer)
        .Include(t => t.Seat)
        .Include(t => t.Event)
        .SingleOrDefaultAsync();

    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        var itemToUpdate = await GetAsync(ticket.Id)
            ?? throw new RecordNotFoundException("The ticket to be updated is not found in the database");

        context.Tickets.Update(ticket);
        await context.SaveChangesAsync();
        return ticket;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Ticket { Id = id };

        context.Tickets.Attach(itemToDelete);
        context.Tickets.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
