using Payment.Demo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Application.Contracts.PaymentServices.Dtos
{
    public class BrainTreePaymentRequestDto
    {
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public string Currency { get; set; } = "USD";
        public string Description { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodNonce { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new();
        public string Email { get; set; }
    }
}
