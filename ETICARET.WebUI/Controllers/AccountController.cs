using ETICARET.Business.Abstract;
using ETICARET.Entities.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            Notify("Oturumunuz kapatıldı.", "info");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmation(string? email)
        {
            var user = string.IsNullOrWhiteSpace(email) ? null : await userManager.FindByEmailAsync(email);

            var sent = true;
            if(user is not null && !await userManager.IsEmailConfirmedAsync(user))
            {
                sent = await SendConfirmationEmailAsync(user);
            }

            if(!sent && environment.IsDevelopment())
            {
                Notify("E-posta gönderilemedi. Onay bağlantısı uygulama konsoluna yazıldı (hata mesajı)", "warning");
            }
            else
            {
                Notify("Bu adres kayıtlı ve onaylanmamışsa onay bağlantısı yeniden gönderildi.", "info");
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgotPassword() => View(new ForgotPasswordModel());

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null && await userManager.IsEmailConfirmedAsync(user))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme)!;

                await emailService.SendAsync(user.Email!, "ETİCARET şifre sıfırlama",
                    EmailTemplate(user.FullName, "Şifrenizi sıfırlamak için aşağıdaki butona tıklayın.", "Şifremi Sıfırla", link));
            }

            // Güvenlik: E-postanın sistemde kayıtlı olup olmadığını belli etmiyoruz (her durumda aynı mesaj).
            Notify("Bu adres kayıtlıysa şifre sıfırlama bağlantısı gönderildi.", "info");
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ResetPassword(string? token, string? email)
        {
            if (token is null)
                return RedirectToAction("Index", "Home");

            return View(new ResetPasswordModel { Token = token, Email = email ?? string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);

            if(user is null)
            {
                ModelState.AddModelError(string.Empty, "Bağlantı süresi dolmuş veya geçersiz");

                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            Notify("Şifreniz güncellendi. Yeni şifrenizle giriş yapabilirsiniz.");
            return RedirectToAction(nameof(Login));

        }


        [Authorize] //Oturum açma zorunluluğu getirir.
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
                return RedirectToAction(nameof(Login));

            return View(new ProfileModel
            {
                FullName = user.FullName,
                UserName = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            });
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Profile(ProfileModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login));

            model.Email = user.Email; // E-posta formdan güncellenmez
            if (!ModelState.IsValid)
                return View(model);

            user.FullName = model.FullName.Trim();
            user.UserName = model.UserName.Trim();
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            // Kullanıcı adı cookie içinde de saklandığı için oturumu tazeliyoruz.
            await signInManager.RefreshSignInAsync(user);
            Notify("Profiliniz güncellendi.");
            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        public IActionResult ChangePassword() => View(new ChangePasswordModel());


        [Authorize, HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);

            if(user is null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await signInManager.RefreshSignInAsync(user);
            Notify("Profiliniz güncellendi.");
            return RedirectToAction(nameof(Profile));
        }
    }

}
