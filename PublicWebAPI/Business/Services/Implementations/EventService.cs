﻿using Common.Models;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class EventService(
    IEventRepository eventRepository,
    IRepository<Seat> seatRepository,
    IRepository<Ticket> ticketRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository ?? throw new ArgumentException(nameof(eventRepository));
    private readonly IRepository<Seat> _seatRepository = seatRepository ?? throw new ArgumentException(nameof(seatRepository));
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository ?? throw new ArgumentException(nameof(ticketRepository));

    public async Task<List<Event>> GetAllAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        foreach (var eventRecord in events)
        {
            eventRecord.Seats = await _seatRepository.GetAllByConditionAsync(seat => seat.EventId == eventRecord.Id);
        }

        return events;
    }

    public async Task<List<SeatWithPricesDto>> GetByIdAndSectionId(int eventId, int sectionId)
    {
        var ticketsByEvent = await _ticketRepository
            .GetAllByConditionAsync(
            ticket => ticket.EventId == eventId,
            ticket => ticket.Seat,
            ticket => ticket.Seat.Section,
            ticket => ticket.PriceOption);

        var seatsWithPricesDtos = new List<SeatWithPricesDto>();

        foreach (var ticket in ticketsByEvent)
        {
            if (ticket.Seat.SectionId == sectionId)
            {
                var seatWithPriceDto = new SeatWithPricesDto()
                {
                    SeatNumber = ticket.Seat.Id,
                    Row = ticket.Seat.RowNumber,
                    Section = ticket.Seat.Section.Name,
                    PriceOption = ticket.PriceOption,
                    TicketStatus = ticket.TicketStatus.ToString(),
                };

                seatsWithPricesDtos.Add(seatWithPriceDto);
            }
        }

        return seatsWithPricesDtos;
    }
}
