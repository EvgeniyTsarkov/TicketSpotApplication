using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

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
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(paymentStatus);
    }


}
