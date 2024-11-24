using Common.Models;
using Common.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public static class DataSeeder
{
    public static ModelBuilder SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventManager>()
            .HasData(
            new EventManager()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@mail.com"
            });

        modelBuilder.Entity<Venue>()
            .HasData(
            new Venue()
            {
                Id = 1,
                Name = "Staples Center",
                Address = "800 W Olympic Blvd #343, Los Angeles, CA 90015",
                Description = "A renowned entertainment venue situated in LA",
                EventManagerId = 1
            });

        modelBuilder.Entity<Event>()
            .HasData(new Event()
            {
                Id = 1,
                Name = "Lakers vs Celtics",
                Date = DateTime.Parse("2024-12-25 19:00:00"),
                Description = "A regular season basketball game",
                EventManagerId = 1,
                VenueId = 1
            });

        modelBuilder.Entity<Section>()
            .HasData(
            new Section { Id = 1, Name = "A" },
            new Section { Id = 2, Name = "B" },
            new Section { Id = 3, Name = "C" },
            new Section { Id = 4, Name = "D" },
            new Section { Id = 5, Name = "E" });

        modelBuilder.Entity<Seat>()
            .HasData(
            new Seat()
            {
                Id = 1,
                SeatNumber = 5,
                SectionId = 1,
                RowNumber = 5,
                EventId = 1,
                VenueId = 1
            },
            new Seat()
            {
                Id = 2,
                SeatNumber = 1,
                SectionId = 3,
                RowNumber = 7,
                EventId = 1,
                VenueId = 1
            });

        modelBuilder.Entity<Customer>()
            .HasData(
            new Customer()
            {
                Id = 1,
                FirstName = "Jack",
                LastName = "Doe",
                Email = "jack.doe@mail.com"
            });

        modelBuilder.Entity<PriceOption>()
            .HasData(
            new PriceOption()
            {
                Id = 1,
                Name = "Discounted Price",
                Price = 25
            },
            new PriceOption()
            {
                Id = 2,
                Name = "Full Price",
                Price = 100
            });

        modelBuilder.Entity<Payment>()
            .HasData(
            new Payment()
            {
                Id = 1,
                Status = PaymentStatus.Pending,
                TotalAmount = 0
            });

        modelBuilder.Entity<Cart>()
            .HasData(
            new Cart()
            {
                Id = Guid.Parse("0a1b428a-9fb0-4ff2-90ef-d3d720304cc0"),
                CartStatus = CartStatus.Empty,
                CustomerId = 1,
                PaymentId = 1
            });

        modelBuilder.Entity<Ticket>()
            .HasData(
            new Ticket()
            {
                Id = 1, 
                PriceOptionId = 1,
                PurchaseDate = DateTime.Parse("2024-10-08"),
                EventId = 1,
                SeatId = 1,
                CustomerId = 1,
                TicketStatus = TicketStatus.Booked,
                CartId = Guid.Parse("0a1b428a-9fb0-4ff2-90ef-d3d720304cc0")
            },
            new Ticket()
            {
                Id = 2,
                PriceOptionId = 2,
                PurchaseDate = DateTime.Parse("2024-10-11"),
                EventId = 1,
                SeatId = 2,
                CustomerId = 1,
                TicketStatus = TicketStatus.Sold,
                CartId = Guid.Parse("0a1b428a-9fb0-4ff2-90ef-d3d720304cc0")
            });

        return modelBuilder;
    }
}
