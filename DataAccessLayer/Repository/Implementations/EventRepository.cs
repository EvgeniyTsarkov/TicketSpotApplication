using Common.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class EventRepository(TicketSpotDbContext context)
    : GenericRepository<Event>(context), IEventRepository
{
    public async Task<List<Event>> GetAllAsync()
    {
        var eventRecords = await _entities
        .AsNoTracking()
        .Include(e => e.EventManager)
        .Include(e => e.Venue)
        .ToListAsync();

        foreach (var eventRecord in eventRecords)
        {
            eventRecord.Seats = eventRecord.Venue.Seats;
        }

        return eventRecords;
    }

    public async Task<Event> GetAsync(int id)
    {
        var eventRecord = await _entities
        .AsNoTracking()
        .Include(e => e.EventManagerId)
        .Include(e => e.Venue)
        .FirstOrDefaultAsync(e => e.Id == id);

        if (eventRecord == null)
        {
            return eventRecord;
        }

        eventRecord.Seats = eventRecord.Venue.Seats;

        return eventRecord;
    }
}
