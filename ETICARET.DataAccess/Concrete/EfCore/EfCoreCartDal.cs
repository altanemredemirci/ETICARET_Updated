using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCartDal(ETicaretContext context) : EfCoreGenericRepository<Cart>(context), ICartDal
    {
        public async Task ClearAsync(int cartId)
            => await Context.CartItems
            .Where(i => i.CartId == cartId)
            .ExecuteDeleteAsync();

        public async Task<Cart?> GetByUserIdAsync(string userId)
            => await Table
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p!.Images.OrderBy(img => img.Id).Take(1))
                .FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task<int> GetItemCountAsync(string userId)
            => await Context.CartItems
                .Where(i => i.Cart!.UserId == userId)
                .SumAsync(i => i.Quantity);
    }
}
