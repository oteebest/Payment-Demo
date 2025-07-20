using Payment.Demo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Core.Dtos.Payments
{
    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
    }
}
