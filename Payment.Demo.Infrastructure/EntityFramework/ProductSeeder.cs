using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Demo.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Infrastructure.EntityFramework
{
    public class ProductSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductSeeder> _logger;

        public ProductSeeder(ApplicationDbContext context, ILogger<ProductSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedProductsAsync()
        {
            try
            {
               
                if (await _context.Products.AnyAsync())
                {
                    _logger.LogInformation("Products already exist. Skipping seeding.");
                    return;
                }

                var products = new List<Product>
            {
                new Product
                {
                    Name = "Nike Kyrie 7 Basketball Shoes",
                    ImageUrl = "/images/nike1.png",
                    Price = 129.99m,
                },
                new Product
                {
                    Name = "Nike Legend Essential Training Shoes",
                    ImageUrl = "/images/nike2.png",
                    Price = 79.99m,
                },
                new Product
                {
                    Name = "Nike SuperRep Go Running Shoes",
                    ImageUrl = "/images/nike3.png",
                    Price = 89.99m,
                }
            };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully seeded {Count} products", products.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding products");
                throw;
            }
        }
    }
}
