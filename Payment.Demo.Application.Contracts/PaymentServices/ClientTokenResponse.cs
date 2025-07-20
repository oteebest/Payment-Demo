
namespace Payment.Demo.Application.Contracts.PaymentServices
{
    public class ClientTokenResponse
    {
        public string ClientToken { get; set; }
        public string CustomerId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
