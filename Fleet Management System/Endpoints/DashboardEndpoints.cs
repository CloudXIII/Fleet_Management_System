using Fleet_Management_System.Services;

namespace Fleet_Management_System.Endpoints;

public static class DashboardEndpoints
{
    public static RouteGroupBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard").WithTags("Dashboard");

        group.MapGet("/stats", async (IDashboardService service) =>
            Results.Ok(await service.GetStatsAsync()));

        return group;
    }
}
