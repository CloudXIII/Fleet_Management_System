namespace Fleet_Management_System.DTOs
{
    public class VehicleComponentReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime InstallationDate { get; set; }
        public int VehicleId { get; set; }
    }
}
