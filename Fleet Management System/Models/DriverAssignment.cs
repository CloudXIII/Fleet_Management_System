using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management_System.Models
{
    public class DriverAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DriverName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        // EndDate is nullable because a driver might still be using the vehicle
        public DateTime? EndDate { get; set; }

        // Foreign Key to Vehicle
        [Required]
        public int VehicleId { get; set; }

        // Navigation property
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }

        // Helper property to check if the assignment is currently active
        [NotMapped]
        public bool IsActive => !EndDate.HasValue || EndDate > DateTime.Now;
    }
}
