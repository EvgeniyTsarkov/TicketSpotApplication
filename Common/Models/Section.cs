namespace Common.Models;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VenueId { get; set; }

    public Venue Venue { get; set; }
    public ICollection<Row> Rows { get; set; } = [];
}
