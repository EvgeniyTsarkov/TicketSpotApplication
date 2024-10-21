using Common.Models;
using PublicWebAPI.Dtos;

namespace PublicWebAPI.Services.Interfaces;

public interface IVenueService
{
    Task<List<Venue>> GetAllAsync();
    Task<SectionsToVenueDto> GetSectionsForVenue(int id);
}
