using Payment.Demo.Application.Contracts.TransactionServices.Dto;
using Payment.Demo.Core.Enums;

namespace Payment.Demo.Application.Contracts.TransactionServices
{
    public interface ITransactionService
    {
        Task<TransactionHistoryDto> GetPaymentHistoryAsync(int pageNumber = 1,
            int pageSize = 20,
            string? searchTerm = null,
            PaymentStatus? status = null);
    }
}
