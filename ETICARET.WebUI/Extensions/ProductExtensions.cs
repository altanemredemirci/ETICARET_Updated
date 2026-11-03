using ETICARET.Entities;

namespace ETICARET.WebUI.Extensions
{
    public static class ProductExtensions
    {
        public const string ImageFolder = "/img/products/";
        public const string Placeholder = "/img/placeholder.jpg";

        public static string ImagePath(string? fileName)
            => string.IsNullOrWhiteSpace(fileName) ? Placeholder : ImageFolder + fileName;

        extension(Product product)
        {
            // İlk resim (yoksa varsayılan görsel)
            public string CoverImage => ImagePath(product.Images.FirstOrDefault()?.ImageUrl);

            public bool InStock => product.Stock > 0;

            // "Son 3 ürün" uyarısı için
            public bool IsLowStock => product.Stock is > 0 and <= 5;
        }

        extension(Image image)
        {
            public string Src => ImagePath(image.ImageUrl);
        }
    }
}
