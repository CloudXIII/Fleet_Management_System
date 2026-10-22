using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DashboardService(ApplicationDbContext context) : IDashboardService
{
    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var warningDate = DateTime.Today.AddDays(30);

        return new DashboardStatsDto
        {
            TotalVehicles = await context.Vehicles.CountAsync(),
            TotalDrivers = await context.Drivers.CountAsync(),
            ActiveAssignments = await context.DriverAssignments.CountAsync(a => a.EndDate == null),
            InsuranceExpiringSoon = await context.Vehicles.CountAsync(v => v.InsuranceExpiry <= warningDate)
        };
    }
}
