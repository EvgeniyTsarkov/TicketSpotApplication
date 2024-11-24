using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Services.Implementations;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Configuration;

public static class ServicesConfiguration
{
    public static void RegisterServices(
        this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IRepository<Venue>, GenericRepository<Venue>>();
        services.AddScoped<IRepository<Seat>, GenericRepository<Seat>>();
        services.AddScoped<IRepository<Ticket>, GenericRepository<Ticket>>();
        services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ITransactionHandler, TransactionHandler>();

        // Register services
        services.AddScoped<IVenueService, VenueService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
    }
}
