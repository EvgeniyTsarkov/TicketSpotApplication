using Common.Models;
using Common.Models.Enums;
using PublicWebAPI.Business.Dtos;

namespace PublicWebAPI.Business.Services.Interfaces;

public interface IOrderService
{
    Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString);
    Task<CartStatus> AddTicketsToCart(string cartIdAsString, OrderPayload oprderPayload);
    Task DeleteSeatFromCart(string cart_id, int event_id, int seat_id);
}
