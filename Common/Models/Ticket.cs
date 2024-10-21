namespace Common.Models;

public class Ticket : IEntity
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public TicketStatus Status { get; set; }
    public int EventId { get; set; }
    public int SeatId { get; set; }
    public int CustomerId { get; set; }

    public Event Event { get; set; }
    public Seat Seat { get; set; }
    public Customer Customer { get; set; }
}
