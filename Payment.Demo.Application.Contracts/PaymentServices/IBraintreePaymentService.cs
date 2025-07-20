using Payment.Demo.Application.Contracts.PaymentServices.Dtos;
using Payment.Demo.Core.Dtos.Payments;
using Payment.Demo.Core.Enums;

namespace Payment.Demo.Application.Contracts.PaymentServices
{
    public interface IBraintreePaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(BrainTreePaymentRequestDto request);
        Task<PaymentStatus> GetPaymentStatusAsync(string transactionId);
        Task<bool> ValidatePaymentAsync(string transactionId);
        Task<string> GenerateClientTokenAsync(string customerId);
    }
}
