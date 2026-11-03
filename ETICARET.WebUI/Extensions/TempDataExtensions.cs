using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace ETICARET.WebUI.Extensions
{
    public static class TempDataExtensions
    {
        extension(ITempDataDictionary tempData)
        {
            public void Put<T>(string key, T value) where T : class
                => tempData[key] = JsonSerializer.Serialize(value);

            public T? Get<T>(string key) where T : class
                => tempData.TryGetValue(key, out var value) && value is string json
                    ? JsonSerializer.Deserialize<T>(json)
                    : null;
        }
    }
}
