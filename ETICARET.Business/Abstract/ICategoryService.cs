using ETICARET.Entities;
using ETICARET.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<List<CategoryWithCount>> GetAllWithCountAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetWithProductAsync(int id);
        Task<int> CountAsync();



    }
}
