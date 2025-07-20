using Microsoft.AspNetCore.Mvc;
using Payment.Demo.Application.Contracts.TransactionServices;
using Payment.Demo.Core.Enums;

namespace Payment.Demo.Controllers
{
    public class TransactionsController(ITransactionService paymentHistoryService) : Controller
    {
        public async Task<IActionResult> Index(int pageNumber = 1,
            int pageSize = 20,
            string? searchTerm = null,
            PaymentStatus? status = null)
        {

            var paymentHistory = await paymentHistoryService.GetPaymentHistoryAsync(
                 pageNumber,
                 pageSize,
                 searchTerm,
                 status);

            return View(paymentHistory);
        }
    }
}
