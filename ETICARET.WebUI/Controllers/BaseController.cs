using ETICARET.Business.Models;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETICARET.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        // Giriş yapmış kullanıcının Id'si (cookie'deki claim'den okunur)
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        // Sağ üstte bildirim göster (bir sonraki sayfada, TempData ile taşınır)
        protected void Notify(string text, string type = "success")
            => TempData.Put(NotifyMessage.Key, new NotifyMessage(text, type));

        protected void Notify(ServiceResult result)
            => Notify(result.Message, result.Success ? "success" : "danger");

        // FluentValidation ile doğrula, hataları ModelState'e ekle.
        protected async Task<bool> ValidateAsync<T>(IValidator<T> validator, T model)
        {
            var result = await validator.ValidateAsync(model);
            result.AddToModelState(ModelState);
            return ModelState.IsValid; // Model binding hataları (ör. fiyata harf yazmak) da hesaba katılır
        }

        // Açık yönlendirme (open redirect) saldırısına karşı: sadece kendi sitemizdeki adreslere dön.
        protected IActionResult RedirectToLocal(string? returnUrl, string fallbackAction = "Index", string fallbackController = "Home")
            => Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl)
                : RedirectToAction(fallbackAction, fallbackController, new { area = "" });
    }
}
