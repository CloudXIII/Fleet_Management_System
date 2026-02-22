using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.DTOs
{
    public class VehicleComponentWriteDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public DateTime InstallationDate { get; set; }

        [Required]
        public int VehicleId { get; set; }
    }
}
