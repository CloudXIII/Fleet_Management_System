using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Endpoints;

public static class VehicleComponentEndpoints
{
    public static RouteGroupBuilder MapVehicleComponentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vehicles/{vehicleId:int}/components").WithTags("Components");

        group.MapGet("/", async (int vehicleId, IVehicleComponentService service) =>
            Results.Ok(await service.GetComponentsAsync(vehicleId)));

        group.MapPost("/", (int vehicleId, VehicleComponentWriteDto dto, IVehicleComponentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                dto.VehicleId = vehicleId;
                var id = await service.CreateComponentAsync(dto);
                return Results.Created($"/api/vehicles/{vehicleId}/components/{id}", new { id });
            }));

        group.MapPut("/{id:int}", (int vehicleId, int id, VehicleComponentWriteDto dto, IVehicleComponentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                dto.VehicleId = vehicleId;
                await service.UpdateComponentAsync(id, dto);
                return Results.NoContent();
            }));

        group.MapDelete("/{id:int}", (int id, IVehicleComponentService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.DeleteComponentAsync(id);
                return Results.NoContent();
            }));

        return group;
    }
}
