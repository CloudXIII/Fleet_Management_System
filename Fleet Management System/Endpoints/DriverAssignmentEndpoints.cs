using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Endpoints;

public static class DriverAssignmentEndpoints
{
    public static RouteGroupBuilder MapDriverAssignmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vehicles/{vehicleId:int}/assignments").WithTags("Assignments");

        group.MapGet("/", async (int vehicleId, IDriverAssignmentService service) =>
            Results.Ok(await service.GetAssignmentsAsync(vehicleId)));

        group.MapPost("/", (int vehicleId, DriverAssignmentWriteDto dto, IDriverAssignmentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                dto.VehicleId = vehicleId;
                var id = await service.CreateAssignmentAsync(dto);
                return Results.Created($"/api/vehicles/{vehicleId}/assignments/{id}", new { id });
            }));

        group.MapPut("/{id:int}", (int vehicleId, int id, DriverAssignmentWriteDto dto, IDriverAssignmentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                dto.VehicleId = vehicleId;
                await service.UpdateAssignmentAsync(id, dto);
                return Results.NoContent();
            }));

        group.MapDelete("/{id:int}", (int id, IDriverAssignmentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.DeleteAssignmentAsync(id);
                return Results.NoContent();
            }));

        group.MapPost("/end-active", (int vehicleId, IDriverAssignmentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.EndActiveAssignmentAsync(vehicleId);
                return Results.NoContent();
            }));

        return group;
    }
}
