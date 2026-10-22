using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class ProductManager(IProductDal productDal) : IProductService
    {
        public async Task<List<Product>> GetAllForAdminAsync()
        => await productDal.GetAllForAdminAsync();

        public async Task<Product?> GetDetailsAsync(int id)
        => await productDal.GetDetailsAsync(id);

        public async Task<List<Product>> GetHomeProductsAsync(int count = 10)
        => await productDal.GetHomeProductsAsync(count);

        public async Task<List<Product>> GetLatestAsync(int count = 10)
        => await productDal.GetLatestAsync(count);

        public async Task<List<Product>> GetLowStockAsync(int threshold)
        => await productDal.GetLowStockAsync(threshold);

        public async Task<PagedResult<Product>> GetProductsAsync(ProductFilter filter)
        => await productDal.GetPagedAsync(filter with { Page = Math.Max(1, filter.Page) });

        public async Task<List<Product>> GetRelatedAsync(int productId, int count = 5)
        => await productDal.GetRelatedAsync(productId, count);
    }
}
