using Common.Models;
namespace PublicWebAPI.Business.Services.Interfaces;

public interface IOrderService
{
    Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString);
}
