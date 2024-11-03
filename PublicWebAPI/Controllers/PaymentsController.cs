using Common.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController(IPaymentService paymentService) : Controller
{
    private readonly IPaymentService _paymentService = paymentService;

    [HttpGet("payments/{paymentIdd:int}")]
    public async Task<IActionResult> GetPaymentStatus(int paymentId)
    {
        PaymentStatus paymentStatus;

        try
        {
            paymentStatus = await _paymentService.GetPaymentStatusAsync(paymentId);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok($"Payment Status: {paymentStatus}");
    }

    [HttpPost("payments/{paymentId:int}/complete")]
    public async Task<IActionResult> UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(int paymentId)
    {
        SeatsToPaymentDto seatsToPaymentDto;

        try
        {
            seatsToPaymentDto = await _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(paymentId, PaymentStatus.Completed, TicketStatus.Sold);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(seatsToPaymentDto);
    }

    [HttpPost("payments/{paymentId:int}/failed")]
    public async Task<IActionResult> UpdatePaymentStatusAndMarkAllRelatedSeatsAsAvailable(int paymentId)
    {
        SeatsToPaymentDto seatsToPaymentDto;

        try
        {
            seatsToPaymentDto = await _paymentService.UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(paymentId, PaymentStatus.Failed, TicketStatus.Available);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(seatsToPaymentDto);
    }
}
