using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class EventRepository(TicketSpotDbContext context) : IRepository<Event>
{
    public new async Task<List<Event>> GetAllAsync() =>
        await context.Events
        .AsNoTracking()
        .AsQueryable()
        .Include(e => e.EventManagerId)
        .Include(e => e.Venue)
        .ToListAsync();

    public async Task<Event> GetAsync(int id) =>
        await context.Events
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(e => e.EventManagerId)
        .Include(e => e.Venue)
        .SingleOrDefaultAsync();

    public async Task<Event> CreateAsync(Event eventToCreate)
    {
        await context.Events.AddAsync(eventToCreate);
        await context.SaveChangesAsync();
        return eventToCreate;
    }

    public async Task<Event> UpdateAsync(Event updatedEvent)
    {
        var itemToUpdate = await GetAsync(updatedEvent.Id)
            ?? throw new RecordNotFoundException("The event to be updated is not found in the database");

        context.Events.Update(updatedEvent);
        await context.SaveChangesAsync();
        return updatedEvent;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Event { Id = id };

        context.Attach(itemToDelete);
        context.Events.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
