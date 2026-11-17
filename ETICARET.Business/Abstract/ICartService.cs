using ETICARET.Business.Models;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task<int> GetItemCountAsync(string userId);
        Task<ServiceResult> AddToCartAsync(string userId, int productId, int quantity);
        Task<ServiceResult> RemoveFromCartAsync(string userId, int productId);
        Task<ServiceResult> UpdateQuantityAsync(string userId, int productId, int quantity);
    }
}
