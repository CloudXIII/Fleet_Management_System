using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.DTOs;

public class VehicleWriteDto
{
    [Required]
    [StringLength(10)]
    public string Plate { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Required]
    public DateTime InsuranceExpiry { get; set; }
}
