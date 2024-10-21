using Common.Models;

namespace PublicWebAPI.Business.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllAsync();
    }
}