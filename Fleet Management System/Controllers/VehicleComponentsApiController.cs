using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet_Management_System.Controllers;

[ApiController]
[Route("api/vehicle-components")]
public class VehicleComponentsApiController(IVehicleComponentService vehicleComponentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleComponentReadDto>>> GetAll([FromQuery] int? vehicleId) =>
        Ok(await vehicleComponentService.GetComponentsAsync(vehicleId));

    [HttpPost]
    public async Task<ActionResult<VehicleComponentReadDto>> Create([FromBody] VehicleComponentWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await vehicleComponentService.CreateComponentAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { vehicleId = created.VehicleId }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleComponentReadDto>> Update(int id, [FromBody] VehicleComponentWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            return Ok(await vehicleComponentService.UpdateComponentAsync(id, dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await vehicleComponentService.DeleteComponentAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
