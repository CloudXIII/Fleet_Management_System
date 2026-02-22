using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Fleet_Management_System.Services;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Extensions;
using Newtonsoft.Json;

namespace Fleet_Management_System.Controllers;

[Route("api/[controller]/[action]")]
public class DriverAssignmentsApiController(IDriverAssignmentService driverAssignmentService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get(int? vehicleId, DataSourceLoadOptions loadOptions) =>
        Json(await driverAssignmentService.GetAssignmentsAsync(vehicleId, loadOptions));

    [HttpPost]
    public async Task<IActionResult> Post(string values)
    {
        var dto = new DriverAssignmentWriteDto();
        JsonConvert.PopulateObject(values, dto);

        if (!TryValidateModel(dto))
            return BadRequest(ModelState.GetFullErrorMessage());

        var id = await driverAssignmentService.CreateAssignmentAsync(dto);
        return Json(new { id });
    }

    [HttpPut]
    public async Task<IActionResult> Put(int key, string values)
    {
        try
        {
            var dto = await driverAssignmentService.GetAssignmentForUpdateAsync(key);
            JsonConvert.PopulateObject(values, dto);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState.GetFullErrorMessage());

            await driverAssignmentService.UpdateAssignmentAsync(key, dto);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int key)
    {
        try
        {
            await driverAssignmentService.DeleteAssignmentAsync(key);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> VehiclesLookup(DataSourceLoadOptions loadOptions) =>
        Json(await driverAssignmentService.GetVehiclesLookupAsync(loadOptions));
}