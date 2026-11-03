using System.Globalization;

namespace ETICARET.WebUI.Extensions
{
    public static class FormatExtensions
    {
        private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");

        extension(decimal price)
        {
            // 1234.5m.ToTL()  →  "₺1.234,50"
            public string ToTL() => price.ToString("C2", Turkish);
        }

        extension(DateTime date)
        {
            // "08 Ekim 2026"
            public string ToTrDate() => date.ToString("dd MMMM yyyy", Turkish);

            // "08 Ekim 2026 14:35"
            public string ToTrDateTime() => date.ToString("dd MMMM yyyy HH:mm", Turkish);
        }

    }
}
