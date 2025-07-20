using Microsoft.EntityFrameworkCore;
using Payment.Demo.Core.Enums;
using Payment.Demo.Domain.Transactions;
using Payment.Demo.Infrastructure.EntityFramework;

namespace Payment.Demo.Infrastructure.Repositories.Transactions
{
    public class TransactionRepository(ApplicationDbContext applicationDbContext) : ITransactionRepository
    {
        public async Task<Transaction> AddAsync(Transaction payment)
        {
            await applicationDbContext.Transactions.AddAsync(payment);
            await applicationDbContext.SaveChangesAsync();

            return payment;
        }

        public async Task UpdateAsync(Transaction payment)
        {
            applicationDbContext.Transactions.Update(payment);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            PaymentStatus? status = null)
        {

            // did raw sql here because sqlite does not support order by Date with ef.

            var query = applicationDbContext.Transactions
            .FromSqlRaw("SELECT * FROM Transactions ORDER BY CreatedAt DESC")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.PaymentMethodNonce.Contains(searchTerm) ||
                    p.CustomerEmail.Contains(searchTerm) ||
                    p.GatewayReference.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(u => u.Product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }


    }
}
