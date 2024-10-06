using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class VenueRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Venue>(ticketSpotContext), IVenueRepository
{
    public async Task<Venue> CreateAsync(Venue venue)
    {
        await _context.Venues.AddAsync(venue);
        await _context.SaveChangesAsync();
        return venue;
    }

    public async Task<Venue> UpdateAsync(Venue venue)
    {
        var itemToUpdate = await Get(x => x.Id == venue.Id)
            ?? throw new RecordNotFoundException("The venue to be updated is not found in the database");

        _context.ChangeTracker.Clear();

        _context.Venues.Update(venue);
        await _context.SaveChangesAsync();
        return venue;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Venue with id: {0} is not found in the database", id));

        _context.ChangeTracker.Clear();

        _context.Venues.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
