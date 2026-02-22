using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string Plate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime InsuranceExpiry { get; set; }

        public virtual ICollection<VehicleComponent> Components { get; set; } = [];
        public virtual ICollection<DriverAssignment> Assignments { get; set; } = [];
    }
}
