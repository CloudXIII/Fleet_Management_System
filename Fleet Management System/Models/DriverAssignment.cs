using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management_System.Models;

public class DriverAssignment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DriverId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    // Null = assignment still active
    public DateTime? EndDate { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [ForeignKey(nameof(VehicleId))]
    public virtual Vehicle? Vehicle { get; set; }

    [ForeignKey(nameof(DriverId))]
    public virtual Driver? Driver { get; set; }
}
