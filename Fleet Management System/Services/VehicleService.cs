using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleService(ApplicationDbContext context, IMapper mapper) : IVehicleService
{
    public async Task<IReadOnlyList<VehicleReadDto>> GetVehiclesAsync() =>
        await context.Vehicles
            .OrderBy(v => v.Id)
            .ProjectTo<VehicleReadDto>(mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<VehicleReadDto?> GetVehicleByIdAsync(int id) =>
        await context.Vehicles
            .Where(v => v.Id == id)
            .ProjectTo<VehicleReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<VehicleWriteDto> GetVehicleForUpdateAsync(int id)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        return mapper.Map<VehicleWriteDto>(entity);
    }

    public async Task<int> CreateVehicleAsync(VehicleWriteDto dto)
    {
        var entity = mapper.Map<Vehicle>(dto);
        context.Vehicles.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateVehicleAsync(int id, VehicleWriteDto dto)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        mapper.Map(dto, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteVehicleAsync(int id)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        context.Vehicles.Remove(entity);
        await context.SaveChangesAsync();
    }
}
