using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController(IPaymentService paymentService) : Controller
{
    private readonly IPaymentService _paymentService = paymentService
        ?? throw new ArgumentNullException(nameof(paymentService));

    [HttpGet("payments/{payment_id:int}")]
    public async Task<IActionResult> GetAll(int payment_id)
    {
        PaymentStatus paymentStatus;

        try
        {
            paymentStatus = await _paymentService.GetPaymentStatusAsync(payment_id);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok($"Payment Status: {paymentStatus}");
    }

    [HttpPost("payments/{payment_id:int}/complete")]
    public async Task<IActionResult> UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(int payment_id)
    {
        SeatsToPaymentDto seatsToPaymentDto;

        try
        {
            seatsToPaymentDto = await _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(payment_id, PaymentStatus.Completed, TicketStatus.Sold);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(seatsToPaymentDto);
    }

    [HttpPost("payments/{payment_id:int}/failed")]
    public async Task<IActionResult> UpdatePaymentStatusAndMarkAllRelatedSeatsAsAvailable(int payment_id)
    {
        SeatsToPaymentDto seatsToPaymentDto;

        try
        {
            seatsToPaymentDto = await _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(payment_id, PaymentStatus.Failed, TicketStatus.Available);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(seatsToPaymentDto);
    }
}
