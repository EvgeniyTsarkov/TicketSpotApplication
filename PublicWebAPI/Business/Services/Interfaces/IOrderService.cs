using Common.Models;
using Common.Models.Enums;
namespace PublicWebAPI.Business.Services.Interfaces;

public interface IOrderService
{
    Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString);
    Task<CartStatus> AddTicketsToCart(string cartIdAsString, OrderPayload oprderPayload);
}
