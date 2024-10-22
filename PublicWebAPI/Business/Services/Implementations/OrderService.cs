using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class OrderService(
    IRepository<Ticket> ticketRepository,
    ICartRepository cartRepository,
    IRepository<TicketStatus> ticketStatusRepository) : IOrderService
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository
        ?? throw new ArgumentNullException(nameof(ticketRepository));
    private readonly ICartRepository _cartRepository = cartRepository
        ?? throw new ArgumentNullException(nameof(cartRepository));
    private readonly IRepository<TicketStatus> _ticketStatusRepository = ticketStatusRepository
        ?? throw new ArgumentNullException(nameof(ticketStatusRepository));

    public async Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString)
    {
        var cardId = ParseCartId(cartIdAsString);

        return await _ticketRepository.GetAllByConditionAsync(ticket => ticket.CartId == cardId);
    }

    public async Task<CartStatus> AddTicketsToCart(string cartIdAsString, OrderPayloadDto orderPayload)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        var ticket = await _ticketRepository.GetByConditionAsync(
            ticket => ticket.EventId == orderPayload.EventId
            && ticket.SeatId == orderPayload.SeatId
            && ticket.PriceOptionId == orderPayload.PriceOptionId)
            ?? throw new RecordNotFoundException($"The requested ticket not found.");

        if (!cart.Tickets.Contains(ticket))
        {
            cart.Tickets.Add(ticket);
        }

        cart.CartStatus = CartStatus.Active;

        await _cartRepository.UpdateAsync(cart);

        return cart.CartStatus;
    }

    public async Task DeleteSeatFromCart(string cartIdAsString, int event_id, int seat_id)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        var ticketToBeDeleted = cart.Tickets.FirstOrDefault(
            ticket => ticket.EventId == event_id
            && ticket.SeatId == seat_id);

        if (ticketToBeDeleted != null)
        {
            cart.Tickets.Remove(ticketToBeDeleted);
            await _cartRepository.UpdateAsync(cart);
        }
    }

    public async Task<List<Ticket>> ChangeStatusOfAllTicketsInCartToBooked(string cartIdAsString)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        var ticketStatuses = await _ticketStatusRepository.GetAllAsync();

        var ticketStatus_Booked = ticketStatuses.FirstOrDefault(ticketStatus => ticketStatus.Name == "Booked")
            ?? throw new RecordNotFoundException("Ticket status 'Booked' does not exist");

        foreach (var ticket in cart.Tickets)
        {
            ticket.TicketStatusId = ticketStatus_Booked.Id;
            ticket.TicketStatus = ticketStatus_Booked;
        }

        await _cartRepository.UpdateAsync(cart);

        return cart.Tickets;
    }

    private static Guid ParseCartId(string cartIdAsString)
    {
        var isParsedCorrectly = Guid.TryParse(cartIdAsString, out var cardId);

        if (!isParsedCorrectly)
        {
            throw new ArgumentException("Incorrect guid as id.");
        }

        return cardId;
    }
}
