using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Endpoints;

public static class VehicleEndpoints
{
    public static RouteGroupBuilder MapVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IVehicleService service) =>
            Results.Ok(await service.GetVehiclesAsync()));

        group.MapGet("/{id:int}", async (int id, IVehicleService service) =>
        {
            var vehicle = await service.GetVehicleByIdAsync(id);
            return vehicle is null
                ? Results.NotFound(new { error = $"Vehicle {id} not found." })
                : Results.Ok(vehicle);
        });

        group.MapPost("/", (VehicleWriteDto dto, IVehicleService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                var id = await service.CreateVehicleAsync(dto);
                var created = await service.GetVehicleByIdAsync(id);
                return Results.Created($"/api/vehicles/{id}", created);
            }));

        group.MapPut("/{id:int}", (int id, VehicleWriteDto dto, IVehicleService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.UpdateVehicleAsync(id, dto);
                return Results.NoContent();
            }));

        group.MapDelete("/{id:int}", (int id, IVehicleService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.DeleteVehicleAsync(id);
                return Results.NoContent();
            }));

        return group;
    }
}
