using ETICARET.Business.Abstract;
using ETICARET.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class CartController(ICartService cartService) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var cart = await cartService.GetCartAsync(UserId) ?? new Cart { UserId = UserId };
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity=1, string? returnUrl = null)
        {
            var result = await cartService.AddToCartAsync(UserId, productId, quantity);
            Notify(result);
            return RedirectToLocal(returnUrl, nameof(Index), "Cart");
        }
    }
}
