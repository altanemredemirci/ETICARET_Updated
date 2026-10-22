using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class CategoryManager(ICategoryDal categoryDal) : ICategoryService
    {
        //public Task<int> CountAsync()
        //{
        //    return categoryDal.CountAsync();
        //}

        public async Task<int> CountAsync()
        => await categoryDal.CountAsync();

        //Sonuçta gelen IEnumrable koleksiyon tipini .. yayma seperatörü ile bir dizi içerisine dağıttık. Böylece gelen sonucu list<Category> haline çevirdik.

        public async Task<List<Category>> GetAllAsync()
        => [..(await categoryDal.GetAllAsync()).OrderBy(c => c.Id)];

        public async Task<List<CategoryWithCount>> GetAllWithCountAsync()
        => await categoryDal.GetAllWithCountAsync();

        public async Task<Category?> GetByIdAsync(int id)
        => await categoryDal.GetByIdAsync(id);

        public async Task<Category?> GetWithProductAsync(int id)
        => await categoryDal.GetWithProductAsync(id);
    }
}
