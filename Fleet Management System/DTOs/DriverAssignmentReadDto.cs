namespace Fleet_Management_System.DTOs
{

    public class DriverAssignmentReadDto
    {
        public int Id { get; set; }
        public string? DriverName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int VehicleId { get; set; }
    }
}
