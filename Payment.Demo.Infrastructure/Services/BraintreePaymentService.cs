using Braintree;
using Braintree.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payment.Demo.Application.Contracts.PaymentServices;
using Payment.Demo.Application.Contracts.PaymentServices.Dtos;
using Payment.Demo.Core.Dtos.Payments;
using Payment.Demo.Core.Enums;
using TransactionStatus = Braintree.TransactionStatus;

namespace Payment.Demo.Infrastructure.Services
{
    public class BraintreePaymentService : IBraintreePaymentService
    {
        private readonly BraintreeGateway _gateway;
        private readonly ILogger<BraintreePaymentService> _logger;

        public BraintreePaymentService(IConfiguration configuration, ILogger<BraintreePaymentService> logger)
        {
            _logger = logger;

            var environment = configuration["Braintree:Environment"]?.ToLower() == "production"
                ? Braintree.Environment.PRODUCTION
                : Braintree.Environment.SANDBOX;

            _gateway = new BraintreeGateway
            {
                Environment = environment,
                MerchantId = configuration["Braintree:MerchantId"],
                PublicKey = configuration["Braintree:PublicKey"],
                PrivateKey = configuration["Braintree:PrivateKey"]
            };
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(BrainTreePaymentRequestDto request)
        {
            try
            {

                var transactionRequest = new TransactionRequest
                {
                    Amount = request.Amount,
                    PaymentMethodNonce = request.PaymentMethodNonce,
                    Customer = new CustomerRequest
                    {
                        Email =  request.Email,
                    },
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true // Auto-capture
                    },
                    CustomFields = request.Metadata
                };


                var result = await _gateway.Transaction.SaleAsync(transactionRequest);

                if (result.IsSuccess())
                {
                    var transaction = result.Target;

                    return new PaymentResultDto
                    {
                        Success = true,
                        TransactionId = transaction.Id,
                        Status = MapBraintreeStatus(transaction.Status),
                        ProcessedAt = transaction.CreatedAt ?? DateTime.UtcNow
                    };
                }
                else
                {
                    var errorMessage = string.Join("; ", result.Errors.All().Select(e => e.Message));
                    _logger.LogError($"Braintree payment failed for oteebest@yahoo.com. Error {errorMessage}");

                    return new PaymentResultDto
                    {
                        Success = false,
                        Status = PaymentStatus.FAILED,
                        ErrorMessage = errorMessage,
                        ProcessedAt = DateTime.UtcNow
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Braintree payment processing for oteebest@yahoo.com");

                return new PaymentResultDto
                {
                    Success = false,
                    Status = PaymentStatus.FAILED,
                    ErrorMessage = "Payment processing failed",
                    ProcessedAt = DateTime.UtcNow
                };
            }
        }

        public async Task<PaymentStatus> GetPaymentStatusAsync(string transactionId)
        {
            try
            {
                var transaction = await _gateway.Transaction.FindAsync(transactionId);
                return MapBraintreeStatus(transaction.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get payment status for transaction {TransactionId}", transactionId);
                return PaymentStatus.ERRORFROMGATEWAY;
            }
        }

        public async Task<bool> ValidatePaymentAsync(string transactionId)
        {
            try
            {
                var transaction = await Task.FromResult(_gateway.Transaction.Find(transactionId));
                return transaction.Status == TransactionStatus.SETTLED ||
                       transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate payment for transaction {TransactionId}", transactionId);
                return false;
            }
        }

        public async Task<string> GenerateClientTokenAsync(string customerId)
        {
            try
            {
                var request = new ClientTokenRequest();
                if (!string.IsNullOrEmpty(customerId))
                {
                    request.CustomerId = customerId;
                }

                return await _gateway.ClientToken.GenerateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate Braintree client token");
                throw;
            }
        }


        private static PaymentStatus MapBraintreeStatus(TransactionStatus braintreeStatus)
        {
            return braintreeStatus switch
            {
                TransactionStatus.AUTHORIZED => PaymentStatus.AUTHORIZED,
                TransactionStatus.AUTHORIZING => PaymentStatus.AUTHORIZING,
                TransactionStatus.AUTHORIZATION_EXPIRED => PaymentStatus.AUTHORIZATION_EXPIRED,
                TransactionStatus.SUBMITTED_FOR_SETTLEMENT => PaymentStatus.SUBMITTED_FOR_SETTLEMENT,
                TransactionStatus.SETTLED => PaymentStatus.SETTLED,
                TransactionStatus.SETTLING => PaymentStatus.SETTLING,
                TransactionStatus.SETTLEMENT_CONFIRMED => PaymentStatus.SETTLEMENT_CONFIRMED,
                TransactionStatus.SETTLEMENT_PENDING => PaymentStatus.SETTLEMENT_PENDING,
                TransactionStatus.SETTLEMENT_DECLINED => PaymentStatus.SETTLEMENT_DECLINED,
                TransactionStatus.FAILED => PaymentStatus.FAILED,
                TransactionStatus.GATEWAY_REJECTED => PaymentStatus.GATEWAY_REJECTED,
                TransactionStatus.PROCESSOR_DECLINED => PaymentStatus.PROCESSOR_DECLINED,
                TransactionStatus.VOIDED => PaymentStatus.VOIDED,
                TransactionStatus.UNRECOGNIZED => PaymentStatus.UNRECOGNIZED,
                _ => PaymentStatus.UKNOWN


            };
        }
    }
}

