using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface ICategoryDal:IRepository<Category>
    {
        Task<List<CategoryWithCount>> GetAllWithCountAsync(); //Kategori içindeki ürün adeti
        Task<Category> GetWithProductAsync(int id); //Kategori ile birlikte ürünleri çek
    }
}
