using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class EventRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Event>(ticketSpotContext), IEventRepository
{
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

        _context.ChangeTracker.Clear();

        _context.Events.Update(updatedEvent);
        await _context.SaveChangesAsync();
        return updatedEvent;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Event with id: {0} is not found in the database", id));

        _context.ChangeTracker.Clear();

        _context.Events.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
