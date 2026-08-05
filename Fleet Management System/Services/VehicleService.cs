using AutoMapper;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Helpers;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleService(ApplicationDbContext context, IMapper mapper) : IVehicleService
{
    public async Task<IReadOnlyList<VehicleReadDto>> GetVehiclesAsync()
    {
        var vehicles = await context.Vehicles
            .AsNoTracking()
            .Include(v => v.Components)
            .Include(v => v.Assignments)
            .OrderBy(v => v.Plate)
            .ToListAsync();

        return vehicles.Select(MapVehicle).ToList();
    }

    public async Task<VehicleReadDto?> GetVehicleByIdAsync(int id)
    {
        var vehicle = await context.Vehicles
            .AsNoTracking()
            .Include(v => v.Components)
            .Include(v => v.Assignments)
            .FirstOrDefaultAsync(v => v.Id == id);

        return vehicle is null ? null : MapVehicle(vehicle);
    }

    public async Task<VehicleWriteDto> GetVehicleForUpdateAsync(int id)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        return mapper.Map<VehicleWriteDto>(entity);
    }

    public async Task<VehicleReadDto> CreateVehicleAsync(VehicleWriteDto dto)
    {
        var entity = mapper.Map<Vehicle>(dto);
        context.Vehicles.Add(entity);
        await context.SaveChangesAsync();
        return (await GetVehicleByIdAsync(entity.Id))!;
    }

    public async Task<VehicleReadDto> UpdateVehicleAsync(int id, VehicleWriteDto dto)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        mapper.Map(dto, entity);
        await context.SaveChangesAsync();
        return (await GetVehicleByIdAsync(id))!;
    }

    public async Task DeleteVehicleAsync(int id)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        context.Vehicles.Remove(entity);
        await context.SaveChangesAsync();
    }

    private static VehicleReadDto MapVehicle(Vehicle vehicle)
    {
        var now = DateTime.UtcNow;
        var activeDriver = vehicle.Assignments
            .Where(a => !a.EndDate.HasValue || a.EndDate > now)
            .OrderByDescending(a => a.StartDate)
            .Select(a => a.DriverName)
            .FirstOrDefault();

        return new VehicleReadDto
        {
            Id = vehicle.Id,
            Plate = vehicle.Plate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            InsuranceExpiry = vehicle.InsuranceExpiry,
            InsuranceStatus = InsuranceHelper.GetStatus(vehicle.InsuranceExpiry),
            ComponentCount = vehicle.Components.Count,
            ActiveDriver = activeDriver
        };
    }
}
