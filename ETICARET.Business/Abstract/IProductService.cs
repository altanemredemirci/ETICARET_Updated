using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface IProductService
    {
        Task<List<Product>> GetHomeProductsAsync(int count = 10);
        Task<List<Product>> GetLatestAsync(int count = 10);
        Task<PagedResult<Product>> GetProductsAsync(ProductFilter filter);
        Task<Product?> GetDetailsAsync(int id);
        Task<List<Product>> GetRelatedAsync(int productId, int count = 5);
        Task<List<Product>> GetAllForAdminAsync();
        Task<List<Product>> GetLowStockAsync(int threshold);
    }
}
