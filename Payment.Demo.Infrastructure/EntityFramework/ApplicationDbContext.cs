using Microsoft.EntityFrameworkCore;
using Payment.Demo.Domain.Products;
using Payment.Demo.Domain.Transactions;
using Payment.Demo.Infrastructure.EntityConfigurations;

namespace Payment.Demo.Infrastructure.EntityFramework
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

        }

        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nike Kyrie 7 Basketball Shoes",
                    ImageUrl = "/images/nike1.jpg",
                    Price = 129.99m,
                },
                new Product
                {
                    Id = 2,
                    Name = "Nike Legend Essential Training Shoes",
                    ImageUrl = "/images/nike2.jpg",
                    Price = 79.99m
                },
                new Product
                {
                    Id = 3,
                    Name = "Nike SuperRep Go Running Shoes",
                    ImageUrl = "/images/nike3.jpg",
                    Price = 89.99m
                }
            );
        }
    }
}
