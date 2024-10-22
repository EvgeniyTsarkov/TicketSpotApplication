using Common.Models.Enums;
using PublicWebAPI.Business.Dtos;

namespace PublicWebAPI.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentStatus> GetPaymentStatusAsync(int id);
        Task<SeatsToPaymentDto> UpdatePaymentStatusAndMarkAllRelatedSeatsAsSold(int payment_id);
    }
}