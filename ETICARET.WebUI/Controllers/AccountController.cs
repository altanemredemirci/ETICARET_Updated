using ETICARET.Business.Abstract;
using ETICARET.Entities.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
         IEmailService emailService,
        IWebHostEnvironment environment) : BaseController
    {

        public IActionResult Login(string? returnUrl = null)
        => View(new LoginModel { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
                return View(model);
            }

            // lockoutOnFailure: true → 5 hatalı denemede hesap 5 dakika kilitlenir (Program.cs)
            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                Notify($"Hoş geldiniz, {user.FullName}!");
                return RedirectToLocal(model.ReturnUrl);
            }

            var error = result switch
            {
                { IsLockedOut: true } => "Çok fazla hatalı deneme yapıldı. Hesabınız 5 dakika kilitlendi.",
                { IsNotAllowed: true } => "Giriş yapabilmek için e-posta adresinizi onaylamalısınız.",
                _ => "E-posta veya şifre hatalı."
            };
            ModelState.AddModelError(string.Empty, error);
            ViewData["ShowResend"] = result.IsNotAllowed; // Onaysız hesap → "Onay e-postasını tekrar gönder" butonu
            return View(model);
        }



        public IActionResult Register() => View(new RegisterModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                FullName = model.FullName.Trim()
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            var sent = await SendConfirmationEmailAsync(user);

            if (sent)
                Notify("Kayıt başarılı! E-posta adresinize gönderilen bağlantı ile hesabınızı onaylayın.");
            else if (environment.IsDevelopment())
                Notify("Kayıt başarılı fakat e-posta gönderilemedi. Onay bağlantısı uygulama konsoluna yazıldı (hata mesajı da orada).", "warning");
            else
                Notify("Kayıt başarılı fakat onay e-postası gönderilemedi. Lütfen yöneticiyle iletişime geçin.", "warning");

            return RedirectToAction(nameof(Login));
        }

        private async Task<bool> SendConfirmationEmailAsync(ApplicationUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            // Request.Scheme verilince tam adres üretilir: https://localhost:7249/Account/ConfirmEmail?userId=...&token=...
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme)!;

            return await emailService.SendAsync(user.Email!, "ETİCARET hesabınızı onaylayın",
                EmailTemplate(user.FullName, "Hesabınızı onaylamak için aşağıdaki butona tıklayın.", "Hesabımı Onayla", link));
        }

        public async Task<IActionResult> ConfirmEmail(string? userId, string? token)
        {
            var user = userId is null ? null : await userManager.FindByIdAsync(userId);
            if (user is null || token is null)
            {
                Notify("Onay bağlantısı geçersiz.", "danger");
                return RedirectToAction("Index", "Home");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            Notify(result.Succeeded
                    ? "E-posta adresiniz onaylandı. Şimdi giriş yapabilirsiniz."
                    : "Onay bağlantısının süresi dolmuş ya da geçersiz.",
                result.Succeeded ? "success" : "danger");

            return RedirectToAction(nameof(Login));
        }

        // NOT (C# 11 raw string literal): """ ... """ arasına HTML'i olduğu gibi yazarız, tırnakları kaçırmaya gerek yok.
        // $$ → süslü parantezler {{değişken}} şeklinde yazılır; tek { } CSS için serbest kalır.
        private static string EmailTemplate(string name, string message, string buttonText, string link) => $$"""
        <div style="font-family:Arial,sans-serif;max-width:520px;margin:auto;padding:24px;border:1px solid #e2e8f0;border-radius:12px">
          <h2 style="color:#4f46e5;margin-top:0">ETİCARET</h2>
          <p>Merhaba {{System.Net.WebUtility.HtmlEncode(name)}},</p>
          <p>{{message}}</p>
          <p style="margin:28px 0">
            <a href="{{link}}" style="background:#4f46e5;color:#fff;padding:12px 22px;border-radius:8px;text-decoration:none">{{buttonText}}</a>
          </p>
          <p style="color:#64748b;font-size:13px">Bu işlemi siz yapmadıysanız bu e-postayı dikkate almayın.</p>
        </div>
        """;
    }

}
