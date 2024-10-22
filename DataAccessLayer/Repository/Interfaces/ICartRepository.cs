using Common.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetAsync(Guid cartId);
        Task<Cart> UpdateAsync(Cart cart);
        Task<Cart> GetByConditionAsync(
            Expression<Func<Cart, bool>> expression,
            params Expression<Func<Cart, object>>[] includes);
    }
}
