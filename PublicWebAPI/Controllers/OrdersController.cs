using Common.Models;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService) : Controller
{
    private readonly IOrderService _orderService = orderService
        ?? throw new ArgumentNullException(nameof(orderService));

    [HttpGet("carts/{cart_id}")]
    public async Task<IActionResult> GetTicketsByCartId(string cart_id)
    {
        var tickets = await _orderService.GetTicketsByCartIdAsync(cart_id);

        return Ok(tickets);
    }

    [HttpPost("carts/{cart_id}")]
    public async Task<IActionResult> AddTicketToCart(string cart_id, OrderPayloadDto orderPayload)
    {
        try
        {
            await _orderService.AddTicketsToCart(cart_id, orderPayload);
        }
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return Created();
    }

    [HttpDelete("orders/carts/{cart_id}/events/{event_id:int}/seats/{seat_id:int}")]
    public async Task<IActionResult> DeleteTicketFromCart(string cart_id, int event_id, int seat_id)
    {
        try
        {
            await _orderService.DeleteSeatFromCart(cart_id, event_id, seat_id);
        }
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    [HttpPut("orders/carts/{cart_id}/book")]
    public async Task<IActionResult> ChangeTicketsStatusToBooked(string cart_id)
    {
        List<Ticket> tickets;

        try
        {
            tickets = await _orderService.ChangeStatusOfAllTicketsInCartToBooked(cart_id);
        }
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(tickets);
    }
}
