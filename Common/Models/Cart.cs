namespace Common.Models;

public class Cart
{
    public Guid Id { get; set; }
    public int PaymentId { get; set; }

    public Payment Payment { get; set; }

    public List<Ticket> Ticket { get; set; } = [];
}
