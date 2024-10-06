using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event> GetAsync(int id);
        Task<Event> CreateAsync(Event eventToCreate);
        Task<Event> UpdateAsync(Event updatedEvent);
        Task DeleteAsync(int id);
    }
}