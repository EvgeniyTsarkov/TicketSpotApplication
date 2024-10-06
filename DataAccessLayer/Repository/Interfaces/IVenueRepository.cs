using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IVenueRepository : IBaseRepository<Venue>
    {
        Task<Venue> CreateAsync(Venue venue);
        Task<Venue> UpdateAsync(Venue venue);
        Task DeleteAsync(int id);
    }
}
