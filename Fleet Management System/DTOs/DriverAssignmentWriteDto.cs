using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.DTOs
{
    public class DriverAssignmentWriteDto
    {
        [Required]
        [StringLength(100)]
        public string DriverName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int VehicleId { get; set; }
    }
}
