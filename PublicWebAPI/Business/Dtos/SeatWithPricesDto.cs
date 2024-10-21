using Common.Models;

namespace PublicWebAPI.Business.Dtos;

public class SeatWithPricesDto
{
    public int SeatNumber { get; set; }
    public int Row { get; set; }
    public string Section { get; set; }
    public decimal Price { get; set; }

    public Status Status { get; set; }
}
