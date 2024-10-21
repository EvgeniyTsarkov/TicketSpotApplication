using Common.Models;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class OrderService(IRepository<Ticket> ticketRepository) : IOrderService
{
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository 
        ?? throw new ArgumentNullException(nameof(ticketRepository));

    public async Task<List<Ticket>> GetTicketsByCartIdAsync(string cartIdAsString)
    {
        var isParsedCorrectly = Guid.TryParse(cartIdAsString, out var cardId);

        if (!isParsedCorrectly)
        {
            throw new ArgumentException("Incorrect guid as id.");
        }

        return await _ticketRepository.GetAllByConditionAsync(ticket => ticket.CartId == cardId);
    }
}
