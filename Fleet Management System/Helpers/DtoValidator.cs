using System.ComponentModel.DataAnnotations;
using Fleet_Management_System.Exceptions;

namespace Fleet_Management_System.Helpers;

public static class DtoValidator
{
    public static void Validate<T>(T dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        if (!Validator.TryValidateObject(dto, context, results, validateAllProperties: true))
        {
            var message = string.Join(" ", results
                .Select(r => r.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m)));

            throw new BusinessException(string.IsNullOrWhiteSpace(message)
                ? "Validation failed."
                : message);
        }
    }
}
