using ETICARET.Business.Abstract;
using ETICARET.Business.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business
{
    public static class BusinessServiceRegistration
    {
        /// <summary>Program.cs'te: builder.Services.AddBusiness();</summary>
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            // Scoped: Her HTTP isteğinde yeni nesne, istek boyunca aynı nesne.
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();

            return services;
        }
    }
}
