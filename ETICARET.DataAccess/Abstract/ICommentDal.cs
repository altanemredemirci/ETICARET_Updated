using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface ICommentDal : IRepository<Comment>
    {
        Task<List<Comment>> GetByProductAsync(int productId);
        Task<List<Comment>> GetAllWithProductAsync();
    }
}
