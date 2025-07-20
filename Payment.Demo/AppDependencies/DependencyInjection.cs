using Microsoft.EntityFrameworkCore;
using Payment.Demo.Application.Contracts.PaymentServices;
using Payment.Demo.Application.Contracts.ProductServices;
using Payment.Demo.Application.Contracts.TransactionServices;
using Payment.Demo.Application.PaymentServices;
using Payment.Demo.Application.ProductServices;
using Payment.Demo.Application.TransactionServices;
using Payment.Demo.Domain.Products;
using Payment.Demo.Domain.Transactions;
using Payment.Demo.Infrastructure.EntityFramework;
using Payment.Demo.Infrastructure.Repositories.Products;
using Payment.Demo.Infrastructure.Repositories.Transactions;
using Payment.Demo.Infrastructure.Services;

namespace Payment.Demo.AppDependencies
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PaymentApplicationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            });

            services.AddScoped<IBraintreePaymentService, BraintreePaymentService>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ProductSeeder>();

            return services;
        }
    }
}
