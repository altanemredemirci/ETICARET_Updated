using ETICARET.DataAccess.Abstract;
using ETICARET.DataAccess.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess
{
    /// <summary>
    /// Her katman kendi servislerini kendisi kaydeder. Program.cs'te tek satır:
    ///     builder.Services.AddDataAccess(connectionString);
    /// NOT: "this IServiceCollection services" → extension method (IServiceCollection'a yeni metot ekler).
    /// </summary>
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
        {
            // DbContext varsayılan olarak Scoped kaydedilir: her HTTP isteği için bir tane.
            services.AddDbContext<ETicaretContext>(options => options.UseSqlServer(connectionString));

            // Interface → Sınıf eşleştirmesi. Controller/Manager "IProductDal ver" dediğinde
            // DI container EfCoreProductDal oluşturup verir.

            services.AddScoped<IProductDal, EfCoreProductDal>();
            //services.AddScoped<ICategoryDal, EfCoreCategoryDal>();


        }
    }
}
