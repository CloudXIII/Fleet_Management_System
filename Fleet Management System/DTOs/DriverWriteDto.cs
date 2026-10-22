using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.DTOs;

public class DriverWriteDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? LicenseNumber { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }
}
