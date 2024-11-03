using Common.Models;
using PublicWebAPI.Business.Dtos;

namespace PublicWebAPI.Business.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllAsync();
        Task<List<SeatWithPricesDto>> GetByIdAndSectionId(int eventId, int sectionId);
    }
}