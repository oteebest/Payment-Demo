using Payment.Demo.Application.Contracts.TransactionServices;
using Payment.Demo.Application.Contracts.TransactionServices.Dto;
using Payment.Demo.Core.Enums;
using Payment.Demo.Domain.Transactions;
using System.Numerics;

namespace Payment.Demo.Application.TransactionServices
{
    public class TransactionService(ITransactionRepository paymentRepository) : ITransactionService
    {

        public async Task<TransactionHistoryDto> GetPaymentHistoryAsync(int pageNumber = 1,
            int pageSize = 20,
            string? searchTerm = null,
            PaymentStatus? status = null)
        {
            var result = await paymentRepository.GetPagedAsync(pageNumber, pageSize, searchTerm, status);

            var paymentHistory = result.Items.Select(u => new TransactionDto
            {
                Id = u.Id,
                Amount = u.Amount,
                Currency = u.Currency,
                Status = u.Status,
                ProcessedAt = u.ProcessedAt,
                ProductId = u.ProductId,
                CustomerEmail = u.CustomerEmail,
                Method = u.Method,
                ProductName = u.Product?.Name,
                CreatedAt = u.CreatedAt,
                GatewayReference = u.GatewayReference,
                ErrorMessage = u.ErrorMessage
            }).ToList();

            return new TransactionHistoryDto
            {
                PaymentHistory = paymentHistory,
                TotalCount = result.TotalCount
            };

        }
    }
}
