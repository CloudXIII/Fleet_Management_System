using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Services;

namespace Fleet_Management_System.Tests;

public class VehicleServiceTests
{
    [Fact]
    public async Task CreateVehicleAsync_PersistsVehicle()
    {
        await using var context = TestHelpers.CreateContext();
        var service = new VehicleService(context, TestHelpers.CreateMapper());

        var id = await service.CreateVehicleAsync(new VehicleWriteDto
        {
            Plate = "ZZ99 AAA",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        });

        var vehicle = await service.GetVehicleByIdAsync(id);
        Assert.NotNull(vehicle);
        Assert.Equal("ZZ99 AAA", vehicle!.Plate);
        Assert.Equal("Ford", vehicle.Brand);
    }

    [Fact]
    public async Task CreateVehicleAsync_DuplicatePlate_ThrowsBusinessException()
    {
        await using var context = TestHelpers.CreateContext();
        var service = new VehicleService(context, TestHelpers.CreateMapper());

        var dto = new VehicleWriteDto
        {
            Plate = "AA11 BBB",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        };

        await service.CreateVehicleAsync(dto);

        var ex = await Assert.ThrowsAsync<BusinessException>(() => service.CreateVehicleAsync(dto));
        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public async Task CreateVehicleAsync_InvalidDto_ThrowsBusinessException()
    {
        await using var context = TestHelpers.CreateContext();
        var service = new VehicleService(context, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<BusinessException>(() => service.CreateVehicleAsync(new VehicleWriteDto
        {
            Plate = "",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        }));
    }

    [Fact]
    public async Task UpdateVehicleAsync_CanChangeDetails()
    {
        await using var context = TestHelpers.CreateContext();
        var service = new VehicleService(context, TestHelpers.CreateMapper());

        var id = await service.CreateVehicleAsync(new VehicleWriteDto
        {
            Plate = "CC22 DDD",
            Brand = "Ford",
            Model = "Transit",
            InsuranceExpiry = DateTime.Today.AddYears(1)
        });

        await service.UpdateVehicleAsync(id, new VehicleWriteDto
        {
            Plate = "CC22 DDD",
            Brand = "Mercedes",
            Model = "Sprinter",
            InsuranceExpiry = DateTime.Today.AddYears(2)
        });

        var vehicle = await service.GetVehicleByIdAsync(id);
        Assert.Equal("Mercedes", vehicle!.Brand);
        Assert.Equal("Sprinter", vehicle.Model);
    }
}
