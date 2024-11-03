using Common.Models;
using Common.Models.Enums;

namespace PublicWebAPI.Business.Dtos;

public class SeatWithPricesDto
{
    public int SeatNumber { get; set; }
    public int Row { get; set; }
    public string Section { get; set; }
    public string TicketStatus { get; set; }

    public PriceOption PriceOption { get; set; }
}
