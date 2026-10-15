using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities.Dtos
{
    /// <summary>
    /// Kategori menüsü ve ana sayfa kartları için: kategori + ürün sayısı + vitrin resmi.
    /// DTO (Data Transfer Object): Ekranın ihtiyacı olan alanları taşır, tablo DEĞİLDİR.
    /// </summary>
    public record CategoryWithCount(int Id, string Name, int ProductCount, string? ImageUrl);
}
