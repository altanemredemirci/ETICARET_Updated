using ETICARET.Business.Abstract;
using ETICARET.Entities.Dtos;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class ShopController(IProductService productService, ICommentService commentService) : BaseController
    {
        private const int PageSize = 12;

        public async Task<IActionResult> List(string? category, string? q, ProductSort sort = ProductSort.Recommended, int page = 1)
        {
            var filter = new ProductFilter(category, q?.Trim(), sort, page, PageSize);

            var result = await productService.GetProductsAsync(filter);

            var model = new ProductListViewModel
            {
                Products = result.Items,
                PageInfo = new PageInfo { TotalItems = result.TotalCount, PageSize = PageSize, CurrentPage = page },
                Category = category,
                Search = q,
                Sort = sort
            };

            return View(model);

        }


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
