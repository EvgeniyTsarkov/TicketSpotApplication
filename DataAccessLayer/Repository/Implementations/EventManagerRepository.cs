using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class EventManagerRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<EventManager>(ticketSpotContext), IEventManagerRepository
{
    public async Task<EventManager> CreateAsync(EventManager eventManager)
    {
        await _context.EventManagers.AddAsync(eventManager);
        await _context.SaveChangesAsync();
        return eventManager;
    }

    public async Task<EventManager> UpdateAsync(EventManager updatedEventManager)
    {
        var itemToUpdate = await Get(x => x.Id == updatedEventManager.Id)
            ?? throw new RecordNotFoundException("The event manager to be updated is not found in the database");

        _context.EventManagers.Update(updatedEventManager);
        await _context.SaveChangesAsync();
        return updatedEventManager;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Event manager with id: {0} is not found in the database", id));

        _context.EventManagers.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
