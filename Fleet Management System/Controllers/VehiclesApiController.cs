using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet_Management_System.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehiclesApiController(IVehicleService vehicleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleReadDto>>> GetAll() =>
        Ok(await vehicleService.GetVehiclesAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleReadDto>> GetById(int id)
    {
        var vehicle = await vehicleService.GetVehicleByIdAsync(id);
        return vehicle is null ? NotFound() : Ok(vehicle);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleReadDto>> Create([FromBody] VehicleWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var created = await vehicleService.CreateVehicleAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleReadDto>> Update(int id, [FromBody] VehicleWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            return Ok(await vehicleService.UpdateVehicleAsync(id, dto));
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
            await vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
