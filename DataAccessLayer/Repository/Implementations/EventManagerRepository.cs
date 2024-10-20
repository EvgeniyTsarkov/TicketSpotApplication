using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class EventManagerRepository(TicketSpotDbContext context) : IRepository<EventManager>
{
    public async Task<List<EventManager>> GetAllAsync() =>
        await context.EventManagers.AsNoTracking().ToListAsync();

    public async Task<EventManager> GetAsync(int id) =>
        await context.EventManagers.AsNoTracking()
        .AsQueryable()
        .Where(c => c.Id == id)
        .SingleOrDefaultAsync();

    public async Task<EventManager> CreateAsync(EventManager eventManager)
    {
        await context.EventManagers.AddAsync(eventManager);
        await context.SaveChangesAsync();
        return eventManager;
    }

    public async Task<EventManager> UpdateAsync(EventManager updatedEventManager)
    {
        var itemToUpdate = await GetAsync(updatedEventManager.Id)
            ?? throw new RecordNotFoundException("The event manager to be updated is not found in the database");

        context.EventManagers.Update(updatedEventManager);
        await context.SaveChangesAsync();
        return updatedEventManager;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new EventManager { Id = id };

        context.Attach(itemToDelete);
        context.EventManagers.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
