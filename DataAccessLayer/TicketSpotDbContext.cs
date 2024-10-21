using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class TicketSpotDbContext(DbContextOptions<TicketSpotDbContext> options) : DbContext(options)
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<EventManager> EventManagers { get; set; }
    public DbSet<Status> Status { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .AddVenueRelationships()
            .AddEventRelationships()
            .AddSeatRelationships()
            .AddTicketRelationships()
            .AddCustomerRelationships();
    }
}
