using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Implementations.Helpers;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        _ = await GetAsync(cart.Id)
            ?? throw new RecordNotFoundException($"Card with id {cart.Id} not found");

        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> GetByConditionAsync(
    Expression<Func<Cart, bool>> expression,
    params Expression<Func<Cart, object>>[] includes)
    {
        var query = _context.Carts.AsNoTracking().Where(expression);

        if (includes.Length != 0)
        {
            query = QueryHelper<Cart>.IncludeMultiple<Cart>(query, includes);
        }

        return await query.FirstOrDefaultAsync();
    }
}
