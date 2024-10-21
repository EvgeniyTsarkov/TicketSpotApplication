using Microsoft.AspNetCore.Mvc;
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
        var tickets = _orderService.GetTicketsByCartIdAsync(cart_id);

        return Ok(tickets);
    }


}
