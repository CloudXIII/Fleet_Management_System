using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Endpoints;

public static class DriverEndpoints
{
    public static RouteGroupBuilder MapDriverEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/drivers").WithTags("Drivers");

        group.MapGet("/", async (IDriverService service) =>
            Results.Ok(await service.GetDriversAsync()));

        group.MapGet("/{id:int}", async (int id, IDriverService service) =>
        {
            var driver = await service.GetDriverByIdAsync(id);
            return driver is null
                ? Results.NotFound(new { error = $"Driver {id} not found." })
                : Results.Ok(driver);
        });

        group.MapPost("/", (DriverWriteDto dto, IDriverService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                var id = await service.CreateDriverAsync(dto);
                var created = await service.GetDriverByIdAsync(id);
                return Results.Created($"/api/drivers/{id}", created);
            }));

        group.MapPut("/{id:int}", (int id, DriverWriteDto dto, IDriverService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.UpdateDriverAsync(id, dto);
                return Results.NoContent();
            }));

        group.MapDelete("/{id:int}", (int id, IDriverService service) =>
            ApiExceptionHandler.ExecuteAsync(async () =>
            {
                await service.DeleteDriverAsync(id);
                return Results.NoContent();
            }));

        return group;
    }
}
