using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Fleet_Management_System.Services;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Extensions;
using Newtonsoft.Json;

namespace Fleet_Management_System.Controllers;

[Route("api/[controller]/[action]")]
public class VehiclesApiController(IVehicleService vehicleService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) =>
        Json(await vehicleService.GetVehiclesAsync(loadOptions));

    [HttpPost]
    public async Task<IActionResult> Post(string values)
    {
        var dto = new VehicleWriteDto();
        JsonConvert.PopulateObject(values, dto);

        if (!TryValidateModel(dto))
            return BadRequest(ModelState.GetFullErrorMessage());

        var id = await vehicleService.CreateVehicleAsync(dto);
        return Json(new { id });
    }

    [HttpPut]
    public async Task<IActionResult> Put(int key, string values)
    {
        try
        {
            var dto = await vehicleService.GetVehicleForUpdateAsync(key);
            JsonConvert.PopulateObject(values, dto);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState.GetFullErrorMessage());

            await vehicleService.UpdateVehicleAsync(key, dto);
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
            await vehicleService.DeleteVehicleAsync(key);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}