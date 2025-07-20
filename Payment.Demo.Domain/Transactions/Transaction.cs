using Payment.Demo.Core.Enums;
using Payment.Demo.Domain.Products;

namespace Payment.Demo.Domain.Transactions
{
    public class Transaction
    {
        public int Id { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string CustomerEmail { get; set; }
        public string? GatewayReference { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }

        //normally i should use a base entity for this
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public PaymentMethod? Method { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTimeOffset ProcessedAt { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
