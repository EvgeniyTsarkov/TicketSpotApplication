using Common.Models;
using PublicWebAPI.Business.Dtos;

namespace PublicWebAPI.Business.Services.Interfaces;

public interface IVenueService
{
    Task<List<Venue>> GetAllAsync();
    Task<SectionsToVenueDto> GetSectionsForVenue(int id);
}
