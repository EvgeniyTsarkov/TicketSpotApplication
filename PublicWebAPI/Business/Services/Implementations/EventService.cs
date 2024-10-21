using Common.Models;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class EventService(
    IEventRepository eventRepository,
    IRepository<Seat> seatRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository ?? throw new ArgumentException(nameof(eventRepository));
    private readonly IRepository<Seat> _seatRepository = seatRepository ?? throw new ArgumentException(nameof(seatRepository));

    public async Task<List<Event>> GetAllAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        foreach (var eventRecord in events)
        {
            eventRecord.Seats = await _seatRepository.GetAllByConditionAsync(seat => seat.EventId == eventRecord.Id);
        }

        return events;
    }
}
