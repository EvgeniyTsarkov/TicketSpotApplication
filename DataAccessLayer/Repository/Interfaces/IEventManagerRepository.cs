using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IEventManagerRepository : IBaseRepository<EventManager>
    {
        Task<EventManager> CreateAsync(EventManager eventManager);
        Task DeleteAsync(int id);
        Task<EventManager> UpdateAsync(EventManager updatedEventManager);
    }
}