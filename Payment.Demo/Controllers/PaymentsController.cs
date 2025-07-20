using Microsoft.AspNetCore.Mvc;
using Payment.Demo.Application.Contracts.PaymentServices;
using Payment.Demo.Application.PaymentServices;
using Payment.Demo.Core.Dtos.Payments;
using Payment.Demo.Domain.Transactions;

namespace Payment.Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(PaymentApplicationService paymentApplicationService,
        ITransactionRepository _paymentRepository,
        IBraintreePaymentService braintreePaymentService,
        ILogger<PaymentsController> logger) : ControllerBase
    {

        [HttpGet("client-token")]
        public async Task<ActionResult<ClientTokenResponse>> GetClientToken()
        {
            //customer id should be the logged in userId
            string customerId = "XXXXXX";

            try
            {
                var clientToken = await braintreePaymentService.GenerateClientTokenAsync(null);

                var response = new ClientTokenResponse
                {
                    ClientToken = clientToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1).AddMinutes(-5), // Braintree tokens usually expires in 24hrs
                    CustomerId = customerId
                };

                logger.LogInformation("Client token generated successfully for customer {CustomerId}", customerId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to generate client token for customer {CustomerId}", customerId);
                return StatusCode(500, "Failed to generate client token");
            }
        }

        [HttpPost("process")]
        public async Task<ActionResult<PaymentResultDto>> ProcessPayment([FromForm] PaymentRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await paymentApplicationService.ProcessPaymentAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing payment for oteebest@yahoo.com");
                return StatusCode(500, "An error occurred while processing the payment");
            }
        }

        //[HttpGet("{transactionId}/status")]
        //public async Task<ActionResult<PaymentStatus>> GetPaymentStatus(string transactionId)
        //{
        //    try
        //    {
        //        var status = await paymentApplicationService.GetPaymentStatusAsync(transactionId);
        //        return Ok(new { TransactionId = transactionId, Status = status });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting payment status for {TransactionId}", transactionId);
        //        return StatusCode(500, "An error occurred while retrieving payment status");
        //    }
        //}

        //[HttpGet]
        //public async Task<ActionResult<PagedResult<Domain.Entities.Payment>>> GetPayments(
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 10,
        //    [FromQuery] string? searchTerm = null,
        //    [FromQuery] PaymentStatus? status = null)
        //{
        //    try
        //    {
        //        if (pageSize > 100) pageSize = 100; // Limit page size

        //        var (payments, totalCount) = await _paymentRepository.GetPagedAsync(
        //            pageNumber, pageSize, searchTerm, status);

        //        var result = new PagedResult<Domain.Entities.Payment>
        //        {
        //            Items = payments,
        //            TotalCount = totalCount,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        //        };

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error retrieving paged payments");
        //        return StatusCode(500, "An error occurred while retrieving payments");
        //    }
        //}
    }
}
