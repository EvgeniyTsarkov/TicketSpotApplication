using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        var cartToBeUpdated = await GetAsync(cart.Id)
            ?? throw new RecordNotFoundException($"Card with id {cart.Id} not found");

        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();

        return cart;
    }
}
