namespace Common.Models;

public class Ticket : IEntity
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int EventId { get; set; }
    public int SeatId { get; set; }
    public int CustomerId { get; set; }
    public int StatusId { get; set; }
    public int PriceOptionId { get; set; }
    public Guid CartId { get; set; }

    public PriceOption PriceOption { get; set; }
    public Event Event { get; set; }
    public Seat Seat { get; set; }
    public Customer Customer { get; set; }
    public Status Status { get; set; }
}
