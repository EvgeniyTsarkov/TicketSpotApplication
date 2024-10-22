using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> AddTicketToCart(string cart_id, OrderPayload orderPayload)
    {
        try
        {
            await _orderService.AddTicketsToCart(cart_id, orderPayload);
        }
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
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
}
