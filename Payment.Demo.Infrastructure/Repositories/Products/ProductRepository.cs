using Microsoft.EntityFrameworkCore;
using Payment.Demo.Domain.Products;
using Payment.Demo.Infrastructure.EntityFramework;

namespace Payment.Demo.Infrastructure.Repositories.Products
{
    public class ProductRepository(ApplicationDbContext applicationDbContext) : IProductRepository
    {
        public async Task<List<Product>> GetAllAsync()
        {
            return await applicationDbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetAsync(int productId)
        {
            return await applicationDbContext.Products.FirstOrDefaultAsync(u => u.Id == productId);
        }
    }
}
