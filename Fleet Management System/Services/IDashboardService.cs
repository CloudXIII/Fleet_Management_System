using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync();
}
