using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Models;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Tests;

public class DriverAssignmentServiceTests
{
    private static async Task<(int VehicleId, int DriverId)> SeedVehicleAndDriverAsync(
        Data.ApplicationDbContext context)
    {
        var vehicle = new Vehicle
        {
            Plate = "TEST 001",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        };
        var driver = new Driver { Name = "Test Driver" };
        context.Vehicles.Add(vehicle);
        context.Drivers.Add(driver);
        await context.SaveChangesAsync();
        return (vehicle.Id, driver.Id);
    }

    [Fact]
    public async Task CreateAssignmentAsync_ActiveAssignment_Succeeds()
    {
        await using var context = TestHelpers.CreateContext();
        var (vehicleId, driverId) = await SeedVehicleAndDriverAsync(context);
        var service = new DriverAssignmentService(context, TestHelpers.CreateMapper());

        var id = await service.CreateAssignmentAsync(new DriverAssignmentWriteDto
        {
            VehicleId = vehicleId,
            DriverId = driverId,
            StartDate = DateTime.Today,
            EndDate = null
        });

        var assignments = await service.GetAssignmentsAsync(vehicleId);
        Assert.Single(assignments);
        Assert.Equal(id, assignments[0].Id);
        Assert.True(assignments[0].IsActive);
    }

    [Fact]
    public async Task CreateAssignmentAsync_SecondActive_ThrowsBusinessException()
    {
        await using var context = TestHelpers.CreateContext();
        var (vehicleId, driverId) = await SeedVehicleAndDriverAsync(context);
        var service = new DriverAssignmentService(context, TestHelpers.CreateMapper());

        await service.CreateAssignmentAsync(new DriverAssignmentWriteDto
        {
            VehicleId = vehicleId,
            DriverId = driverId,
            StartDate = DateTime.Today.AddMonths(-1),
            EndDate = null
        });

        var secondDriver = new Driver { Name = "Another Driver" };
        context.Drivers.Add(secondDriver);
        await context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<BusinessException>(() => service.CreateAssignmentAsync(
            new DriverAssignmentWriteDto
            {
                VehicleId = vehicleId,
                DriverId = secondDriver.Id,
                StartDate = DateTime.Today,
                EndDate = null
            }));

        Assert.Contains("already has an active", ex.Message);
    }

    [Fact]
    public async Task CreateAssignmentAsync_EndBeforeStart_ThrowsBusinessException()
    {
        await using var context = TestHelpers.CreateContext();
        var (vehicleId, driverId) = await SeedVehicleAndDriverAsync(context);
        var service = new DriverAssignmentService(context, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<BusinessException>(() => service.CreateAssignmentAsync(
            new DriverAssignmentWriteDto
            {
                VehicleId = vehicleId,
                DriverId = driverId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(-1)
            }));
    }

    [Fact]
    public async Task CreateAssignmentAsync_EndedThenNewActive_Succeeds()
    {
        await using var context = TestHelpers.CreateContext();
        var (vehicleId, driverId) = await SeedVehicleAndDriverAsync(context);
        var service = new DriverAssignmentService(context, TestHelpers.CreateMapper());

        await service.CreateAssignmentAsync(new DriverAssignmentWriteDto
        {
            VehicleId = vehicleId,
            DriverId = driverId,
            StartDate = DateTime.Today.AddMonths(-6),
            EndDate = DateTime.Today.AddMonths(-1)
        });

        var secondDriver = new Driver { Name = "Replacement Driver" };
        context.Drivers.Add(secondDriver);
        await context.SaveChangesAsync();

        var id = await service.CreateAssignmentAsync(new DriverAssignmentWriteDto
        {
            VehicleId = vehicleId,
            DriverId = secondDriver.Id,
            StartDate = DateTime.Today,
            EndDate = null
        });

        Assert.True(id > 0);
        var assignments = await service.GetAssignmentsAsync(vehicleId);
        Assert.Equal(2, assignments.Count);
        Assert.Equal(1, assignments.Count(a => a.IsActive));
    }

    [Fact]
    public async Task EndActiveAssignmentAsync_SetsEndDateToToday()
    {
        await using var context = TestHelpers.CreateContext();
        var (vehicleId, driverId) = await SeedVehicleAndDriverAsync(context);
        var service = new DriverAssignmentService(context, TestHelpers.CreateMapper());

        await service.CreateAssignmentAsync(new DriverAssignmentWriteDto
        {
            VehicleId = vehicleId,
            DriverId = driverId,
            StartDate = DateTime.Today.AddDays(-10),
            EndDate = null
        });

        await service.EndActiveAssignmentAsync(vehicleId);

        var assignments = await service.GetAssignmentsAsync(vehicleId);
        Assert.All(assignments, a => Assert.False(a.IsActive));
        Assert.Equal(DateTime.Today, assignments[0].EndDate);
    }
}
