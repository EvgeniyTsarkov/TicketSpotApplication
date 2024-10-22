using Common.Models.Enums;

namespace PublicWebAPI.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentStatus> GetPaymentStatusAsync(int id);
    }
}