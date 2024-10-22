using Common.Models;

namespace PublicWebAPI.Business.Dtos;

public class SeatWithPricesDto
{
    public int SeatNumber { get; set; }
    public int Row { get; set; }
    public string Section { get; set; }

    public PriceOption PriceOption { get; set; }
    public TicketStatus Status { get; set; }
}
