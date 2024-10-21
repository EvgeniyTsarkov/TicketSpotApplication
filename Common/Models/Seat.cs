namespace Common.Models;

public class Seat : IEntity
{
    public int Id { get; set; }
    public int SeatNumber { get; set; }
    public char Section { get; set; }
    public int RowNumber { get; set; }
    public int VenueId { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}
