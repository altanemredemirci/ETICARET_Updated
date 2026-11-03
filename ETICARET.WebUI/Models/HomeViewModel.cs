using ETICARET.Entities;
using ETICARET.Entities.Dtos;

namespace ETICARET.WebUI.Models
{
    public record HomeViewModel(
        List<CategoryWithCount> Categories,
        List<Product> FeaturedProducts,
        List<Product> LastestProducts);

    public class ProductListViewModel
    {
        public required List<Product> Products { get; set; }
        public required PageInfo PageInfo { get; set; }
        public string? Category { get; set; }
        public string? Search { get; set; }
        public ProductSort Sort { get; set; }

        public string Title => (Category, Search) switch
        {
            (_, { Length: > 0 } search) => $"\"{search}\" için sonuçlar",
            ({ Length: > 0 } category, _) => Category,
            _ => "Tüm Ürünler"
        };
    }

    public record CategoryMenuViewModel(List<CategoryWithCount> Categories, string? SelectedCategory);
}
