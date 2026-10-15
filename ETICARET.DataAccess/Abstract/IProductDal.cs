using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface IProductDal:IRepository<Product>
    {
        Task<List<Product>> GetHomeProductsAsync(int count);
        Task<List<Product>> GetLatestAsync(int count);
        Task<PagedResult<Product>> GetPagedAsync(ProductFilter filter);
        Task<Product?> GetDetailsAsync(int id);
        Task<List<Product>> GetRelatedAsync(int productId, int count);
        Task<List<Product>> GetAllForAdminAsync();
        Task<List<Product>> GetLowStockAsync(int threshold);
    }
}
