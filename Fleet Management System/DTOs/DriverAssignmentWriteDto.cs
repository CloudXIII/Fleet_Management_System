using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.DTOs;

public class DriverAssignmentWriteDto : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Driver is required.")]
    public int DriverId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Vehicle is required.")]
    public int VehicleId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate.HasValue && EndDate.Value.Date < StartDate.Date)
        {
            yield return new ValidationResult(
                "End date must be on or after the start date.",
                [nameof(EndDate)]);
        }
    }
}
