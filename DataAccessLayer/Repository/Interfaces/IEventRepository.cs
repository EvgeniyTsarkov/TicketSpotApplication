using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<Event> CreateAsync(Event eventToCreate);
        Task<Event> UpdateAsync(Event updatedEvent);
        Task DeleteAsync(int id);
    }
}