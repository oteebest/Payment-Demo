using Payment.Demo.Core.Enums;

namespace Payment.Demo.Domain.Transactions
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction payment);
        Task UpdateAsync(Transaction payment);
        Task<(IEnumerable<Transaction> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            PaymentStatus? status = null);
    }
}
