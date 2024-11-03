using Common.Models.Enums;

namespace Common.Models;

public class Cart
{
    public Guid Id { get; set; }
    public CartStatus CartStatus { get; set; } = CartStatus.Empty;
    public int CustomerId { get; set; }
    public int PaymentId { get; set; }

    public Payment Payment { get; set; }
    public Customer Customer { get; set; }

    public List<Ticket> Tickets { get; set; } = [];
}
