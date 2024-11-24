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
        var cart = await _context.Carts
             .AsNoTracking()
             .Include(c => c.Customer)
             .Include(c => c.Payment)
             .FirstOrDefaultAsync(cart => cart.Id == cartId);

        if (cart != null)
        {
            var tickets = await _context.Tickets.
                AsNoTracking()
                .Where(t => t.CartId == cartId)
                .Include(t => t.PriceOption)
                .ToListAsync();

            cart.Tickets = tickets;
        }

        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        _ = _context.Set<Cart>().AsNoTracking().FirstOrDefault(e => e.Id == cart.Id)
            ?? throw new RecordNotFoundException("The cart to be updated is not found in the database");

        var local = _context.Set<Cart>().Local.FirstOrDefault(entry => entry.Id.Equals(cart.Id));
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Entry(cart).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
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

    public async Task DeleteAsync(Guid id)
    {
        var itemToDelete = new Cart { Id = id };

        var local = _context.Set<Cart>().Local.FirstOrDefault(entry => entry.Id.Equals(id));
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Entry(itemToDelete).State = EntityState.Deleted;

        await _context.SaveChangesAsync();
    }
}
