using ETICARET.Business.Models;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface ICommentService
    {
        Task<List<Comment>> GetByProductAsync(int productId);
        Task<List<Comment>> GetAllAsync();
        Task<ServiceResult> AddAsync(int productId, string userId, string userName, string text);
        Task<ServiceResult> UpdateAsync(int commentId, string userId, string text);
        Task<ServiceResult> DeleteAsync(int commentId, string userId, bool isAdmin);
    }
}
