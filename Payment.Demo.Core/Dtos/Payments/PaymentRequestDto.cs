using Payment.Demo.Core.Enums;

namespace Payment.Demo.Core.Dtos.Payments
{
    public class PaymentRequestDto
    {
        public int ProductId { get; set; }
        public string Currency { get; set; } = "USD";
        public string Description { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodNonce { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}
