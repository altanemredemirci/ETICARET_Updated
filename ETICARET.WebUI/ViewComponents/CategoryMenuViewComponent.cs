using ETICARET.Business.Abstract;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.ViewComponents
{
    public class CategoryMenuViewComponent(ICategoryService categoryService):ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string view = "Default")
        {
            var model = new CategoryMenuViewModel(
                await categoryService.GetAllWithCountAsync(),
                RouteData.Values["category"]?.ToString());

            return View(view, model);
        }
    }
}
