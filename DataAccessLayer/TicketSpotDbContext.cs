using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class TicketSpotDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EventManager> EventManagers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO Move the connection string to configs
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TicketSpotDb;Trusted_Connection=True;");
        }

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
}
