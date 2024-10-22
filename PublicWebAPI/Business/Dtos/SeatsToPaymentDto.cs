using Common.Models;

namespace PublicWebAPI.Business.Dtos;

public class SeatsToPaymentDto
{
    public Payment Payment { get; set; }
    public List<Ticket> Tickets { get; set; }
}
