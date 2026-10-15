using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities.Dtos
{
    /// <summary>Sayfalı sorgu sonucu: o sayfadaki kayıtlar + toplam kayıt sayısı.</summary>
    public record PagedResult<T>(List<T> Items, int TotalCount);
    
}
