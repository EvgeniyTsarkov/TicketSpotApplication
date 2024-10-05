namespace Common.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int EventManagerId { get; set; }

    public Venue Venue { get; set; }
    public EventManager EventManager { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = [];
}
