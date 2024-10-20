using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event> GetAsync(int id);
        Task<Event> CreateAsync(Event entity);
        Task<Event> UpdateAsync(Event entity);
        Task DeleteAsync(int id);
    }
}
