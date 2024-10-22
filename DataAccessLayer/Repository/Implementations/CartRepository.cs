using Common.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class CartRepository(TicketSpotDbContext ticketSpotDbContext) : ICartRepository
{
    private readonly TicketSpotDbContext _context = ticketSpotDbContext
            ?? throw new ArgumentNullException(nameof(ticketSpotDbContext));

    public async Task<Cart> GetAsync(Guid cartId)
    {
        return await _context.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(cart => cart.Id == cartId);
    }
}
