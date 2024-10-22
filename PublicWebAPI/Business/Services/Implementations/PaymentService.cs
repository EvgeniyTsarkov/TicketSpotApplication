using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class PaymentService(
    IRepository<Payment> paymentRepository,
    ICartRepository cartRepository,
    IRepository<Ticket> ticketRepository)
    : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository
        ?? throw new ArgumentException(nameof(paymentRepository));
    private readonly ICartRepository _cartRepository = cartRepository
        ?? throw new ArgumentException(nameof(cartRepository));
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository
        ?? throw new ArgumentException(nameof(ticketRepository));

    public async Task<PaymentStatus> GetPaymentStatusAsync(int id)
    {
        var payment = await _paymentRepository.GetAsync(id)
            ?? throw new RecordNotFoundException($"Payment with id {id} not found");

        return payment.Status;
    }

    public async Task<SeatsToPaymentDto> UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(
        int payment_id,
        PaymentStatus paymentStatus,
        TicketStatus ticketStatus)
    {
        var payment = await _paymentRepository.GetAsync(payment_id)
            ?? throw new RecordNotFoundException($"Payment with id {payment_id} was not found");

        var cart = await _cartRepository.GetByConditionAsync(cart => cart.PaymentId == payment_id)
            ?? throw new RecordNotFoundException($"Cart with payment id {payment_id} was not found");

        payment.Status = paymentStatus;

        await _paymentRepository.UpdateAsync(payment);

        var tickets = await _ticketRepository.GetAllByConditionAsync(
            ticket => ticket.CartId == cart.Id);

        foreach (var ticket in tickets)
        {
            ticket.TicketStatus = ticketStatus;
            await _ticketRepository.UpdateAsync(ticket);
        }

        var ticketsResult = await _ticketRepository.GetAllByConditionAsync(
            ticket => ticket.CartId == cart.Id);

        return new SeatsToPaymentDto
        {
            Status = payment.Status.ToString(),
            Tickets = ticketsResult
        };
    }
}
