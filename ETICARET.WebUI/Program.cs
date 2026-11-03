using ETICARET.Business;
using ETICARET.DataAccess;
using ETICARET.WebUI.Data;
using System.Globalization;

namespace ETICARET.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Sayı biçimi: Formdan gelen "1250.50" gibi ondalıklı değerler her bilgisayarda aynı okunsun diye
            // sabit (Invariant) kültür kullanılır. Ekrandaki TL ve tarih biçimi: Extensions/FormatExtensions.cs
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataAccess(builder.Configuration.GetConnectionString("DefaultConnection")!);
            builder.Services.AddBusiness();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            await app.InitializeDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
