using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetAsync(Guid cartId);
        Task<Cart> UpdateAsync(Cart cart);
    }
}
