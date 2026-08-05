using Fleet_Management_System.DTOs;
using Fleet_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet_Management_System.Controllers;

[ApiController]
[Route("api/driver-assignments")]
public class DriverAssignmentsApiController(IDriverAssignmentService driverAssignmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DriverAssignmentReadDto>>> GetAll([FromQuery] int? vehicleId) =>
        Ok(await driverAssignmentService.GetAssignmentsAsync(vehicleId));

    [HttpPost]
    public async Task<ActionResult<DriverAssignmentReadDto>> Create([FromBody] DriverAssignmentWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await driverAssignmentService.CreateAssignmentAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { vehicleId = created.VehicleId }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DriverAssignmentReadDto>> Update(int id, [FromBody] DriverAssignmentWriteDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            return Ok(await driverAssignmentService.UpdateAssignmentAsync(id, dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await driverAssignmentService.DeleteAssignmentAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
