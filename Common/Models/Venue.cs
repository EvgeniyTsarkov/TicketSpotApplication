namespace Common.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public int EventManagerId { get; set; }

        public EventManager EventManager { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
