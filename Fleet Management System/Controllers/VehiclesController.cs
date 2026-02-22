using Microsoft.AspNetCore.Mvc;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Controllers;

public class VehiclesController(IVehicleService vehicleService) : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Details(int id)
    {
        var vehicle = await vehicleService.GetVehicleByIdAsync(id);

        if (vehicle == null)
            return NotFound();

        return View(vehicle);
    }
}