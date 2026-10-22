using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Helpers;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleComponentService(ApplicationDbContext context, IMapper mapper) : IVehicleComponentService
{
    public async Task<IReadOnlyList<VehicleComponentReadDto>> GetComponentsAsync(int vehicleId) =>
        await context.VehicleComponents
            .AsNoTracking()
            .Where(c => c.VehicleId == vehicleId)
            .OrderBy(c => c.Id)
            .ProjectTo<VehicleComponentReadDto>(mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<VehicleComponentWriteDto> GetComponentForUpdateAsync(int id)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        return mapper.Map<VehicleComponentWriteDto>(entity);
    }

    public async Task<int> CreateComponentAsync(VehicleComponentWriteDto dto)
    {
        DtoValidator.Validate(dto);
        await EnsureVehicleExistsAsync(dto.VehicleId);

        var entity = mapper.Map<VehicleComponent>(dto);
        context.VehicleComponents.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateComponentAsync(int id, VehicleComponentWriteDto dto)
    {
        DtoValidator.Validate(dto);

        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        await EnsureVehicleExistsAsync(dto.VehicleId);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteComponentAsync(int id)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        context.VehicleComponents.Remove(entity);
        await context.SaveChangesAsync();
    }

    private async Task EnsureVehicleExistsAsync(int vehicleId)
    {
        var exists = await context.Vehicles.AnyAsync(v => v.Id == vehicleId);
        if (!exists)
            throw new KeyNotFoundException($"Vehicle {vehicleId} not found.");
    }
}
