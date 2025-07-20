using Microsoft.AspNetCore.Mvc;

namespace Payment.Demo.Controllers
{
    public class SuccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
