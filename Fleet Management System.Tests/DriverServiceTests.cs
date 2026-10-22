using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Models;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Tests;

public class DriverServiceTests
{
    [Fact]
    public async Task CreateDriverAsync_PersistsDriver()
    {
        await using var context = TestHelpers.CreateContext();
        var service = new DriverService(context, TestHelpers.CreateMapper());

        var id = await service.CreateDriverAsync(new DriverWriteDto
        {
            Name = "Alex Driver",
            LicenseNumber = "LIC123",
            Phone = "07700900111"
        });

        var driver = await service.GetDriverByIdAsync(id);
        Assert.NotNull(driver);
        Assert.Equal("Alex Driver", driver!.Name);
    }

    [Fact]
    public async Task DeleteDriverAsync_WithAssignments_ThrowsBusinessException()
    {
        await using var context = TestHelpers.CreateContext();
        var vehicle = new Vehicle
        {
            Plate = "DEL 001",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        };
        var driver = new Driver { Name = "Busy Driver" };
        context.Vehicles.Add(vehicle);
        context.Drivers.Add(driver);
        await context.SaveChangesAsync();

        context.DriverAssignments.Add(new DriverAssignment
        {
            VehicleId = vehicle.Id,
            DriverId = driver.Id,
            StartDate = DateTime.Today,
            EndDate = null
        });
        await context.SaveChangesAsync();

        var service = new DriverService(context, TestHelpers.CreateMapper());
        var ex = await Assert.ThrowsAsync<BusinessException>(() => service.DeleteDriverAsync(driver.Id));
        Assert.Contains("assignments", ex.Message);
    }
}
