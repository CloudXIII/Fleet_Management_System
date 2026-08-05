using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleComponentService(ApplicationDbContext context, IMapper mapper) : IVehicleComponentService
{
    public async Task<IReadOnlyList<VehicleComponentReadDto>> GetComponentsAsync(int? vehicleId)
    {
        var query = context.VehicleComponents.AsNoTracking().AsQueryable();

        if (vehicleId.HasValue)
            query = query.Where(c => c.VehicleId == vehicleId.Value);

        return await query
            .OrderBy(c => c.Name)
            .ProjectTo<VehicleComponentReadDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<VehicleComponentWriteDto> GetComponentForUpdateAsync(int id)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        return mapper.Map<VehicleComponentWriteDto>(entity);
    }

    public async Task<VehicleComponentReadDto> CreateComponentAsync(VehicleComponentWriteDto dto)
    {
        await EnsureVehicleExists(dto.VehicleId);

        var entity = mapper.Map<VehicleComponent>(dto);
        context.VehicleComponents.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<VehicleComponentReadDto>(entity);
    }

    public async Task<VehicleComponentReadDto> UpdateComponentAsync(int id, VehicleComponentWriteDto dto)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        await EnsureVehicleExists(dto.VehicleId);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync();

        return mapper.Map<VehicleComponentReadDto>(entity);
    }

    public async Task DeleteComponentAsync(int id)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        context.VehicleComponents.Remove(entity);
        await context.SaveChangesAsync();
    }

    private async Task EnsureVehicleExists(int vehicleId)
    {
        if (!await context.Vehicles.AnyAsync(v => v.Id == vehicleId))
            throw new KeyNotFoundException($"Vehicle {vehicleId} not found.");
    }
}
