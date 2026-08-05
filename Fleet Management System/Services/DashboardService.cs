using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DashboardService(ApplicationDbContext context) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var vehicles = await context.Vehicles.AsNoTracking().ToListAsync();
        var componentsCount = await context.VehicleComponents.CountAsync();
        var assignments = await context.DriverAssignments
            .AsNoTracking()
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();

        var alerts = vehicles
            .Select(v => new VehicleAlertDto
            {
                Id = v.Id,
                Plate = v.Plate,
                Brand = v.Brand,
                Model = v.Model,
                InsuranceExpiry = v.InsuranceExpiry,
                InsuranceStatus = InsuranceHelper.GetStatus(v.InsuranceExpiry, now)
            })
            .Where(v => v.InsuranceStatus != InsuranceStatus.Valid)
            .OrderBy(v => v.InsuranceExpiry)
            .ToList();

        return new DashboardDto
        {
            TotalVehicles = vehicles.Count,
            ActiveAssignments = assignments.Count(a => !a.EndDate.HasValue || a.EndDate > now),
            ExpiringInsuranceCount = alerts.Count(a => a.InsuranceStatus == InsuranceStatus.ExpiringSoon),
            ExpiredInsuranceCount = alerts.Count(a => a.InsuranceStatus == InsuranceStatus.Expired),
            TotalComponents = componentsCount,
            VehiclesByBrand = vehicles
                .GroupBy(v => v.Brand)
                .Select(g => new BrandCountDto { Brand = g.Key, Count = g.Count() })
                .OrderByDescending(b => b.Count)
                .ThenBy(b => b.Brand)
                .ToList(),
            InsuranceAlerts = alerts,
            RecentAssignments = assignments
                .Take(6)
                .Select(a => new RecentAssignmentDto
                {
                    Id = a.Id,
                    DriverName = a.DriverName,
                    VehiclePlate = a.Vehicle?.Plate ?? "—",
                    StartDate = a.StartDate,
                    IsActive = !a.EndDate.HasValue || a.EndDate > now
                })
                .ToList()
        };
    }
}
