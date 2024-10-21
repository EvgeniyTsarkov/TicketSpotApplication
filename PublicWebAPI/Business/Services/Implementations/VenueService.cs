using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class VenueService(
    IRepository<Venue> venueRepository,
    IRepository<Seat> seatRepository)
    : IVenueService
{
    private readonly IRepository<Venue> _venueRepository = venueRepository ?? throw new ArgumentNullException(nameof(venueRepository));
    private readonly IRepository<Seat> _seatRepository = seatRepository ?? throw new ArgumentException(nameof(seatRepository));

    public async Task<List<Venue>> GetAllAsync()
    {
        var venues = await _venueRepository.GetAllAsync(x => x.EventManager);

        foreach (var venue in venues)
        {
            venue.Seats = await _seatRepository.GetAllByConditionAsync(seat => seat.VenueId == venue.Id);
        }

        return venues;
    }

    public async Task<SectionsToVenueDto> GetSectionsForVenue(int id)
    {
        var venue = await _venueRepository.GetAsync(id);

        if (venue == null)
        {
            throw new RecordNotFoundException($"Record with id: {id} was not found");
        }

        var seats = await _seatRepository.GetAllByConditionAsync(x => x.VenueId == 2);

        var sections = seats.Select(seat => seat.Section).ToArray();

        var sectionsToVenueDto = new SectionsToVenueDto()
        {
            VenueName = venue.Name,
            Sections = sections
        };

        return sectionsToVenueDto;
    }
}
