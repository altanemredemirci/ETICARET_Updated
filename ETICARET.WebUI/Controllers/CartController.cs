using ETICARET.Business.Abstract;
using ETICARET.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class CartController(ICartService cartService) : BaseController
    {
        [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var result = await cartService.UpdateQuantityAsync(UserId, productId, quantity);

            if (!result.Success)
            {
                Notify(result);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            Notify(await cartService.RemoveFromCartAsync(UserId, productId));
            return RedirectToAction(nameof(Index));
        }
    }
}
