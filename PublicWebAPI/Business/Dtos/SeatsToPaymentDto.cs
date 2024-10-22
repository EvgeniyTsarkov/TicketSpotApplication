using Common.Models;

namespace PublicWebAPI.Business.Dtos;

public class SeatsToPaymentDto
{
    public string Status { get; set; }
    public List<Ticket> Tickets { get; set; }
}
