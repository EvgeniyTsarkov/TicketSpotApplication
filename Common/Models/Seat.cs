namespace Common.Models;

public class Seat
{
    public int Id { get; set; }
    public string Section { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public int VenueId { get; set; }

    public Venue Venue { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = [];
}
