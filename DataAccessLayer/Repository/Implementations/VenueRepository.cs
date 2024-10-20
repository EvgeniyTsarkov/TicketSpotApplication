using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class VenueRepository(TicketSpotDbContext context) : IRepository<Venue>
{
    public async Task<List<Venue>> GetAllAsync() =>
        await context.Venues
        .AsNoTracking()
        .AsQueryable()
        .Include(e => e.EventManagerId)
        .ToListAsync();

    public async Task<Venue> GetAsync(int id) =>
        await context.Venues
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(e => e.EventManagerId)
        .SingleOrDefaultAsync();

    public async Task<Venue> CreateAsync(Venue venue)
    {
        await context.Venues.AddAsync(venue);
        await context.SaveChangesAsync();
        return venue;
    }

    public async Task<Venue> UpdateAsync(Venue venue)
    {
        var itemToUpdate = await GetAsync(venue.Id)
            ?? throw new RecordNotFoundException("The venue to be updated is not found in the database");

        context.Venues.Update(venue);
        await context.SaveChangesAsync();
        return venue;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Venue { Id = id };

        context.Attach(itemToDelete);
        context.Venues.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
