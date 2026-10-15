using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    /// <summary>
    /// Generic Repository: Bütün tablolar için ortak CRUD (Create-Read-Update-Delete) işlemleri.
    /// "where T : BaseEntity" → T sadece BaseEntity'den türeyen bir sınıf olabilir (generic kısıt).
    /// Database de kullanılacak temel metotları her entity için ayrı ayrı yazmak yerine genel bir tanım ile taslak oluşturuyoruz
    /// </summary>
    public interface IRepository<T> where T: class
    {
        Task<T?> GetByIdAsync(int id);

        //Expression=> i=> i.Id>3
        // filter verilmezse hepsi gelir. Örnek: GetAllAsync(p => p.Stock > 0)
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

    }
}
