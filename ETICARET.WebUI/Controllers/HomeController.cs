using ETICARET.Business.Abstract;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ETICARET.WebUI.Controllers
{
    public class HomeController(IProductService productService,ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel(
                Categories: await categoryService.GetAllWithCountAsync(),
                FeaturedProducts: await productService.GetHomeProductsAsync(),
                LastestProducts: await productService.GetLatestAsync(5));

            return View(model);
        }

     

    }
}
