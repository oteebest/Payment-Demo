using Microsoft.AspNetCore.Mvc;
using Payment.Demo.Application.Contracts.ProductServices;
using Payment.Demo.Models;
using System.Diagnostics;

namespace Payment.Demo.Controllers
{
    public class ProductsController(IProductService productService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetAsync(id);
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
