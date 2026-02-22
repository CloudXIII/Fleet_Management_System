using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fleet_Management_System.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetFullErrorMessage(this ModelStateDictionary modelState)
        {
            var messages = modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return string.Join(" ", messages);
        }
    }
}