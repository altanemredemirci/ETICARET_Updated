using ETICARET.Business;
using ETICARET.Business.Settings;
using ETICARET.DataAccess;
using ETICARET.DataAccess.Concrete.EfCore;
using ETICARET.Entities.Identity;
using ETICARET.WebUI.Data;
using ETICARET.WebUI.Infrastucture;
using Microsoft.AspNetCore.Identity;
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

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.User.RequireUniqueEmail = true;
                //options.User.AllowedUserNameCharacters += "çğşı";

                options.SignIn.RequireConfirmedEmail = true;

            })
                .AddEntityFrameworkStores<ETicaretContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<TurkishIdentityErrorDescriber>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
                options.Cookie.Name = ".Eticaret.Auth";
                options.Cookie.HttpOnly = true;
            });


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
