using ETICARET.Business.Abstract;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class ShopController(IProductService productService, ICommentService commentService) : BaseController
    {
        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetDetailsAsync(id);
            if (product is null)
                return NotFound(); // → UseStatusCodePages → 404 sayfası

            var model = new ProductDetailsViewModel(
                product,
                await productService.GetRelatedAsync(id),
                new CommentListViewModel(id, await commentService.GetByProductAsync(id)));

            return View(model);
        }
    }
}
