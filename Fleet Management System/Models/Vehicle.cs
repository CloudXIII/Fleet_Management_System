using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.Models;

public class Vehicle
{
    [Key]
    public int Id { get; set; }

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

    public virtual ICollection<VehicleComponent> Components { get; set; } = [];
    public virtual ICollection<DriverAssignment> Assignments { get; set; } = [];
}
