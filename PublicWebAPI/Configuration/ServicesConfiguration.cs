using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Services.Implementations;
using PublicWebAPI.Services.Interfaces;

namespace PublicWebAPI.Configuration;

public static class ServicesConfiguration
{
    public static void RegisterServices(
        this IServiceCollection services)
    {
        services.AddScoped<IRepository<Venue>, GenericRepository<Venue>>();
        services.AddScoped<IRepository<Seat>, GenericRepository<Seat>>();

        services.AddScoped<IVenueService, VenueService>();
    }
}
