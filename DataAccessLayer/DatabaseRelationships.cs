using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public static class DatabaseRelationships
    {
        public static ModelBuilder AddVenueRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Seats)
                .WithOne(s => s.Venue)
                .HasForeignKey(s => s.VenueId);

            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Events)
                .WithOne(e => e.Venue)
                .HasForeignKey(v => v.EventManagerId);

            modelBuilder.Entity<Venue>()
                .HasOne(v => v.EventManager)
                .WithMany(em => em.Venues)
                .HasForeignKey(v => v.EventManagerId);

            return modelBuilder;
        }

        public static ModelBuilder AddEventRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventManager)
                .WithMany(em => em.Events)
                .HasForeignKey(e => e.EventManagerId);

            return modelBuilder;
        }

        public static ModelBuilder AddSeatRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Seat)
                .HasForeignKey(t => t.SeatId);

            return modelBuilder;
        }

        public static ModelBuilder AddTicketRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId);

            return modelBuilder;
        }

        public static ModelBuilder AddCustomerRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Customer)
                .HasForeignKey(t => t.CustomerId);

            return modelBuilder;
        }
    }
}
