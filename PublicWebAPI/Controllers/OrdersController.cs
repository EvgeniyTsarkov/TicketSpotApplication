using Common.Models;
using Common.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService) : Controller
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet("carts/{cartId}")]
    public async Task<IActionResult> GetTicketsByCartId(string cartId)
    {
        var tickets = await _orderService.GetTicketsByCartIdAsync(cartId);

        return Ok(tickets);
    }

    [HttpPost("carts/{cartId}")]
    public async Task<IActionResult> AddTicketToCart(string cartId, OrderPayloadDto orderPayload)
    {
        CartStatus cartStatus;

        try
        {
            cartStatus = await _orderService.AddTicketsToCartAsync(cartId, orderPayload);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(cartStatus);
    }

    [HttpDelete("carts/{cartId}/events/{eventId:int}/seats/{seatId:int}")]
    public async Task<IActionResult> DeleteTicketFromCart(string cartId, int eventId, int seatId)
    {
        try
        {
            await _orderService.DeleteSeatFromCartAsync(cartId, eventId, seatId);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpPut("carts/{cartId}/book")]
    public async Task<IActionResult> ChangeTicketsStatusToBooked(string cartId)
    {
        List<Ticket> tickets;

        try
        {
            tickets = await _orderService.ChangeStatusOfAllTicketsInCartToBooked(cartId);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok(tickets);
    }
}
