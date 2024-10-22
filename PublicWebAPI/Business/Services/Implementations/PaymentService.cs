using Common.Models;
using Common.Models.Enums;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Business.Services.Implementations;

public class PaymentService(IRepository<Payment> paymentRepository) : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository
        ?? throw new ArgumentException(nameof(paymentRepository));

    public async Task<PaymentStatus> GetPaymentStatusAsync(int id)
    {
        var payment = await _paymentRepository.GetAsync(id)
            ?? throw new RecordNotFoundException($"Payment with id {id} not found");

        return payment.Status;
    }
}
