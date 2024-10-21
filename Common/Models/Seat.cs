namespace Common.Models;

public class Seat : IEntity
{
    public int Id { get; set; }
    public int SeatNumber { get; set; }
    public int RowNumber { get; set; }
    public int VenueId { get; set; }
    public int EventId { get; set; }
    public int SectionId { get; set; }

    public Section Section { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}
