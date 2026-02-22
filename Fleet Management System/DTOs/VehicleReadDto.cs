namespace Fleet_Management_System.DTOs
{
    public class VehicleReadDto
    {
        public int? Id { get; set; }
        public string Plate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime InsuranceExpiry { get; set; }
    }
}
