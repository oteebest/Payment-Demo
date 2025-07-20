namespace Payment.Demo.Domain.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetAsync(int productId);
    }
}
