using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IVenueRepository
    {
        Task<List<Venue>> GetAllAsync();
        Task<Venue> GetAsync(int id);
        Task<Venue> CreateAsync(Venue venue);
        Task<Venue> UpdateAsync(Venue venue);
        Task DeleteAsync(int id);
    }
}
