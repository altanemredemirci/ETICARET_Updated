using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface ICartDal:IRepository<Cart>
    {
        Task<Cart> GetByUserIdAsync(string userId);
        Task<int> GetItemCountAsync(string userId);
        Task ClearAsync(int cartId);
    }
}
