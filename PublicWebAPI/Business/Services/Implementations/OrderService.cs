using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class OrderService(
    IRepository<Ticket> ticketRepository,
    IRepository<Payment> paymentRepository,
    ICartRepository cartRepository,
    ITransactionHandler transactionHandler) : IOrderService
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly ITransactionHandler _transactionHandler = transactionHandler;

    public async Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString)
    {
        var cardId = ParseCartId(cartIdAsString);

        return await _ticketRepository.GetAllByConditionAsync(ticket => ticket.CartId == cardId);
    }

    public async Task<CartStatus> AddTicketsToCartAsync(string cartIdAsString, OrderPayloadDto orderPayload)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        var ticket = await _ticketRepository.GetByConditionAsync(
            ticket => ticket.EventId == orderPayload.EventId
            && ticket.SeatId == orderPayload.SeatId
            && ticket.PriceOptionId == orderPayload.PriceOptionId,
            t => t.PriceOption)
            ?? throw new RecordNotFoundException($"The requested ticket not found.");

        await _transactionHandler.BeginTransactionAsync();

        try
        {
            ticket.TicketStatus = TicketStatus.Booked;
            ticket.CartId = cartId;
            ticket.CustomerId = cart.CustomerId;
            await _ticketRepository.UpdateAsync(ticket);

            var updatedPayment = cart.Payment;
            updatedPayment.TotalAmount += ticket.PriceOption.Price;
            updatedPayment.Status = PaymentStatus.Pending;
            await _paymentRepository.UpdateAsync(updatedPayment);

            cart.CartStatus = CartStatus.Active;
            await _cartRepository.UpdateAsync(cart);

            await _transactionHandler.CommitAsync();
        }
        catch (Exception)
        {
            await _transactionHandler.RollbackAsync();
            throw;
        }

        return cart.CartStatus;
    }

    public async Task DeleteSeatFromCartAsync(string cartIdAsString, int event_id, int seat_id)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        var ticketToBeDeleted = cart.Tickets.FirstOrDefault(
            ticket => ticket.EventId == event_id
            && ticket.SeatId == seat_id);

        if (ticketToBeDeleted == null)
        {
            return;
        }

        await _transactionHandler.BeginTransactionAsync();

        try
        {
            ticketToBeDeleted.CustomerId = null;
            ticketToBeDeleted.TicketStatus = TicketStatus.Available;
            ticketToBeDeleted.CartId = null;

            await _ticketRepository.UpdateAsync(ticketToBeDeleted);

            cart.Tickets.Remove(ticketToBeDeleted);

            if (cart.Tickets.IsNullOrEmpty())
            {
                cart.CartStatus = CartStatus.Empty;
            }

            await _cartRepository.UpdateAsync(cart);

            var payment = await _paymentRepository.GetAsync(cart.PaymentId);

            if (payment != null)
            {
                payment.TotalAmount -= ticketToBeDeleted.PriceOption.Price;

                if (payment.TotalAmount == 0)
                {
                    payment.Status = PaymentStatus.Cancelled;
                }

                await _paymentRepository.UpdateAsync(payment);
            }

            await _transactionHandler.CommitAsync();
        }
        catch (Exception ex)
        {
            await _transactionHandler.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Ticket>> ChangeStatusOfAllTicketsInCartToBooked(string cartIdAsString)
    {
        var cartId = ParseCartId(cartIdAsString);

        var cart = await _cartRepository.GetAsync(cartId)
            ?? throw new RecordNotFoundException($"Cart with id {cartIdAsString} not found.");

        foreach (var ticket in cart.Tickets)
        {
            ticket.TicketStatus = TicketStatus.Booked;
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
