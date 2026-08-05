namespace Fleet_Management_System.DTOs;

public class DashboardDto
{
    public int TotalVehicles { get; set; }
    public int ActiveAssignments { get; set; }
    public int ExpiringInsuranceCount { get; set; }
    public int ExpiredInsuranceCount { get; set; }
    public int TotalComponents { get; set; }
    public IReadOnlyList<BrandCountDto> VehiclesByBrand { get; set; } = [];
    public IReadOnlyList<VehicleAlertDto> InsuranceAlerts { get; set; } = [];
    public IReadOnlyList<RecentAssignmentDto> RecentAssignments { get; set; } = [];
}

public class BrandCountDto
{
    public string Brand { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class VehicleAlertDto
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime InsuranceExpiry { get; set; }
    public InsuranceStatus InsuranceStatus { get; set; }
}

public class RecentAssignmentDto
{
    public int Id { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }
}
