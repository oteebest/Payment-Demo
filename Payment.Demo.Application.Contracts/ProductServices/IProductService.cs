using Payment.Demo.Application.Contracts.ProductServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Application.Contracts.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetAsync(int productId);
    }
}
