namespace Payment.Demo.Application.Contracts.TransactionServices.Dto
{
    public class TransactionHistoryDto
    {
        public List<TransactionDto> PaymentHistory { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
