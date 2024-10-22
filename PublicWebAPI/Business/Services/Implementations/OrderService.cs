using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class OrderService(
    IRepository<Ticket> ticketRepository,
    ICartRepository cartRepository) : IOrderService
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository
        ?? throw new ArgumentNullException(nameof(ticketRepository));
    private readonly ICartRepository _cartRepository = cartRepository
        ?? throw new ArgumentNullException(nameof(cartRepository));

    public async Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString)
    {
        var cardId = ParseCartId(cartIdAsString);

        return await _ticketRepository.GetAllByConditionAsync(ticket => ticket.CartId == cardId);
    }

    public async Task<CartStatus> AddTicketsToCart(string cartIdAsString, OrderPayload orderPayload)
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

        return cart.CartStatus;
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
