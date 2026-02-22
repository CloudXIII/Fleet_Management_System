using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Fleet_Management_System.Services;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Extensions;
using Newtonsoft.Json;

namespace Fleet_Management_System.Controllers;

[Route("api/[controller]/[action]")]
public class VehicleComponentsApiController(IVehicleComponentService vehicleComponentService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get(int? vehicleId, DataSourceLoadOptions loadOptions) =>
        Json(await vehicleComponentService.GetComponentsAsync(vehicleId, loadOptions));

    [HttpPost]
    public async Task<IActionResult> Post(string values)
    {
        var dto = new VehicleComponentWriteDto();
        JsonConvert.PopulateObject(values, dto);

        if (!TryValidateModel(dto))
            return BadRequest(ModelState.GetFullErrorMessage());

        var id = await vehicleComponentService.CreateComponentAsync(dto);
        return Json(new { id });
    }

    [HttpPut]
    public async Task<IActionResult> Put(int key, string values)
    {
        try
        {
            var dto = await vehicleComponentService.GetComponentForUpdateAsync(key);
            JsonConvert.PopulateObject(values, dto);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState.GetFullErrorMessage());

            await vehicleComponentService.UpdateComponentAsync(key, dto);
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
            await vehicleComponentService.DeleteComponentAsync(key);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> VehiclesLookup(DataSourceLoadOptions loadOptions) =>
        Json(await vehicleComponentService.GetVehiclesLookupAsync(loadOptions));
}