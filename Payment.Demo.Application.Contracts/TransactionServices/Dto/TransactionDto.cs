using Payment.Demo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Application.Contracts.TransactionServices.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string CustomerEmail { get; set; }
        public PaymentMethod? Method { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? GatewayReference { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTimeOffset ProcessedAt { get; set; }
        public string Currency { get; set; }
        public string? ProductName { get; set; }
        public int ProductId { get; set; }
        public string MethodText => Method?.ToString() ?? "Unknown";
        public string PaymentStatusText => Status.ToString();

        public bool ShouldFulfillOrder => Status switch
        {
            PaymentStatus.SUBMITTED_FOR_SETTLEMENT => true,
            PaymentStatus.SETTLING => true,
            PaymentStatus.SETTLED => true,
            PaymentStatus.SETTLEMENT_CONFIRMED => true,
            // Include AUTHORIZED if you want to fulfill on authorization (riskier)
            // PaymentStatus.AUTHORIZED => true,
            _ => false
        };
    }
}
