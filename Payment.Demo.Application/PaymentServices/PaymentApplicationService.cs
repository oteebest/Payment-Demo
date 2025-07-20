using Microsoft.Extensions.Logging;
using Payment.Demo.Application.Contracts.PaymentServices;
using Payment.Demo.Application.Contracts.PaymentServices.Dtos;
using Payment.Demo.Core.Dtos.Payments;
using Payment.Demo.Core.Enums;
using Payment.Demo.Domain.Products;
using Payment.Demo.Domain.Transactions;

namespace Payment.Demo.Application.PaymentServices
{
    public class PaymentApplicationService(IBraintreePaymentService paymentService,
        ITransactionRepository paymentRepository,
        IProductRepository productRepository,
        ILogger<PaymentApplicationService> logger)
    {

        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            try
            {

                // get product amount;
                var product = await productRepository.GetAsync(request.ProductId);

                if(product == null)
                {
                    throw new Exception($"User is trying to pay for invalid product. product Id{request.ProductId}");
                }

                var amount = product.Price;

                //log request
                var transaction = new Transaction
                {
                    Amount = amount,
                    ProductId = product.Id,
                    Currency = request.Currency,
                    CustomerEmail = "oteebest@yahoo.com",
                    Status = PaymentStatus.PENDING,
                    Method = request.PaymentMethod,
                    PaymentMethodNonce = request.PaymentMethodNonce
                };

                await paymentRepository.AddAsync(transaction);

                // Call the payment gateway
                var result = await paymentService.ProcessPaymentAsync(
                    new BrainTreePaymentRequestDto
                    {
                        Amount = amount,
                        Currency = request.Currency,
                        PaymentMethodNonce = transaction.PaymentMethodNonce,
                        PaymentMethod = request.PaymentMethod,
                        Metadata = request.Metadata,
                        Email = "oteebest@yahoo.com"
                    });

                // update payments

                transaction.PaymentMethodNonce = result.TransactionId;
                transaction.Status = result.Status;
                transaction.GatewayReference = result.TransactionId;
                transaction.ProcessedAt = result.ProcessedAt;
                transaction.ErrorMessage = result.ErrorMessage;

                await paymentRepository.UpdateAsync(transaction);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing payment for oteebest@yahoo.com");
                throw;
            }
        }
    }
}
