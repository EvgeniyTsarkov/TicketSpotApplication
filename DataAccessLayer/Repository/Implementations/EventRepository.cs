using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class EventRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Event>(ticketSpotContext), IEventRepository
{
    public new async Task<List<Event>> GetAllAsync() =>
        await _context.Events
        .AsNoTracking()
        .AsQueryable()
        .Include(e => e.EventManagerId)
        .Include(e => e.Venue)
        .ToListAsync();

    public async Task<Event> GetAsync(int id) =>
        await _context.Events
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(e => e.EventManagerId)
        .Include(e => e.Venue)
        .SingleOrDefaultAsync();

    public async Task<Event> CreateAsync(Event eventToCreate)
    {
        await _context.Events.AddAsync(eventToCreate);
        await _context.SaveChangesAsync();
        return eventToCreate;
    }

    public async Task<Event> UpdateAsync(Event updatedEvent)
    {
        var itemToUpdate = await Get(x => x.Id == updatedEvent.Id)
            ?? throw new RecordNotFoundException("The event to be updated is not found in the database");

        _context.Events.Update(updatedEvent);
        await _context.SaveChangesAsync();
        return updatedEvent;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await GetAsync(id)
            ?? throw new RecordNotFoundException(string.Format("Event with id: {0} is not found in the database", id));

        _context.Events.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
