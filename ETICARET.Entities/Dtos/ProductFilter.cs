using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities.Dtos
{
    /// <summary>
    /// Ürün listeleme sayfasının filtreleri.
    /// NOT (C# 9): "record" → sadece veri taşıyan, değiştirilemeyen (immutable) küçük tip.
    /// Parantez içindeki parametreler otomatik olarak özelliğe dönüşür (positional record).
    /// </summary>
    public record ProductFilter(
        string? Category = null,
        string? Search = null,
        ProductSort Sort = ProductSort.Recommended,
        int Page = 1,
        int PageSize = 12);

    public enum  ProductSort
    {
        Recommended,
        PriceAsc,
        PriceDesc,
        Newest
    }
}
