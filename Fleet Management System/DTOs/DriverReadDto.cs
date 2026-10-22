namespace Fleet_Management_System.DTOs;

public class DriverReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LicenseNumber { get; set; }
    public string? Phone { get; set; }
}
