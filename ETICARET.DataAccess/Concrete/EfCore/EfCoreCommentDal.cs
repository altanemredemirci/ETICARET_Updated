using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCommentDal(ETicaretContext context)
     : EfCoreGenericRepository<Comment>(context), ICommentDal
    {
        public async Task<List<Comment>> GetByProductAsync(int productId)
            => await Table.AsNoTracking()
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.Id)
                .ToListAsync();

        public async Task<List<Comment>> GetAllWithProductAsync()
            => await Table.AsNoTracking()
                .Include(c => c.Product)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
    }
}
