using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace PublicWebAPI.Configuration;

public static class DataAccessConfiguration
{
    public static void RegisterDatabase(
        this IServiceCollection services,
        AppSettings configuration) =>
        services.AddDbContext<TicketSpotDbContext>(options =>
        options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection));
}
