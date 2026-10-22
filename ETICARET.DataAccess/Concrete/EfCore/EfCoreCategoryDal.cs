using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryDal(ETicaretContext context) : EfCoreGenericRepository<Category>(context), ICategoryDal
    {
        //Select: Bütün ürünleri belleğe çekmek yarine sadece
        //ihtiyacımız olan sayıyı ve ilk resmi SQL tarafında hesaplatıyoruz.
        public async Task<List<CategoryWithCount>> GetAllWithCountAsync()
        => await Table.AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoryWithCount( //Bulduğun kayıtlardan CategoryWithCount nesnesi oluştur. 
                c.Id,
                c.Name,
                c.Products.Count,
                c.Products.OrderByDescending(p => p.IsHome)
                    .SelectMany(p => p.Images.OrderBy(i => i.Id).Take(1))
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()))
            .ToListAsync();

        public async Task<Category?> GetWithProductAsync(int id)
        => await Table
            .Include(c => c.Products)
            .ThenInclude(p => p.Images.OrderBy(i => i.Id).Take(1))
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
