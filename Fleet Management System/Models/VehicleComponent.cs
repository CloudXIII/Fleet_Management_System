using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management_System.Models
{
    public class VehicleComponent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public DateTime InstallationDate { get; set; }

        // Foreign Key to Vehicle
        [Required]
        public int VehicleId { get; set; }

        // Navigation property
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }
    }
}
