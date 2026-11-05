using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation.Results;

namespace ETICARET.WebUI.Extensions
{
    public static class ValidationExtensions
    {
        extension(ValidationResult result)
        {
            /// <summary>
            /// FluentValidation hatalarını ModelState'e aktarır;
            /// böylece view'daki asp-validation-for etiketleri mesajları gösterir.
            /// </summary>
            public void AddToModelState(ModelStateDictionary modelState)
            {
                foreach (var error in result.Errors)
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
