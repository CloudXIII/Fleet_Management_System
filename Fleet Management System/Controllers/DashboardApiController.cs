using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet_Management_System.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardApiController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get() =>
        Ok(await dashboardService.GetDashboardAsync());
}
