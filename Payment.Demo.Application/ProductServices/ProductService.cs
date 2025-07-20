using Payment.Demo.Application.Contracts.ProductServices;
using Payment.Demo.Application.Contracts.ProductServices.Dto;
using Payment.Demo.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Application.ProductServices
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<List<ProductDto>> GetAllAsync()
        {
            // we can use automapper for this or we create an extension method for mapping products to dto

            return (await productRepository.GetAllAsync() )
                .Select(u => new ProductDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    ImageUrl = u.ImageUrl,
                    Price = u.Price
                }).ToList();         
        }

        public async Task<ProductDto?> GetAsync(int productId)
        {
            var product = await productRepository.GetAsync(productId);

            if(product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            };
        }
    }
}
