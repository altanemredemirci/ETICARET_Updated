using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    /// <summary>
    /// IRepository'nin EF Core ile yazılmış hali. Diğer Dal sınıfları bundan miras alır.
    ///
    /// NOT: Eskiden her metotta "using var context = new DataContext()" yazılıyordu.
    /// Artık DbContext Dependency Injection ile (constructor'dan) gelir. Bir HTTP isteği boyunca
    /// tek bir DbContext kullanılır (Scoped). Böylece farklı Dal'lar aynı context'i paylaşır.
    /// </summary>
    public class EfCoreGenericRepository<TEntity>(ETicaretContext context) : IRepository<TEntity>
        where TEntity : BaseEntity
    {

        // Alt sınıflar (EfCoreProductDal vb.) da kullanabilsin diye protected.
        protected ETicaretContext Context { get; } = context;

        protected DbSet<TEntity> Table => Context.Set<TEntity>();

        public virtual async Task AddAsync(TEntity entity)
        {
            Table.Add(entity);
            await Context.SaveChangesAsync();
        }

        //public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter)
        //{
        //    return await Table.AnyAsync(filter);
        //}

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter)
        => await Table.AnyAsync(filter);



        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        => filter is null
            ? await Table.CountAsync()
            : await Table.CountAsync(filter);
            

        public virtual async Task DeleteAsync(TEntity entity)
        {
            Table.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        => filter is null
            ? await Table.ToListAsync()
            : await Table.Where(filter).ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        => await Table.FindAsync(id);

        public virtual async Task UpdateAsync(TEntity entity)
        {
            // Change Tracking: Veritabanından okunmuş (takip edilen) bir nesnede yapılan
            // değişiklikleri EF zaten bilir; sadece SaveChanges yeterlidir.
            // Formdan yeni oluşturulmuş (takip edilmeyen) nesne ise Update ile bildiririz.
            if (Context.Entry(entity).State == EntityState.Modified)
                Table.Update(entity);

            await Context.SaveChangesAsync();
        }
    }
}
