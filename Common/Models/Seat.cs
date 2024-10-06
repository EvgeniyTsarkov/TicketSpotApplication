namespace Common.Models;

public class Seat
{
    public int Id { get; set; }
    public int SeatNumber { get; set; }
    public int RowId { get; set; }

    public Row Row { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = [];
}
