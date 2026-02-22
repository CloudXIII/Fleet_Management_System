using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleComponentService(ApplicationDbContext context, IMapper mapper) : IVehicleComponentService
{
    public async Task<LoadResult> GetComponentsAsync(int? vehicleId, DataSourceLoadOptions loadOptions)
    {
        var query = context.VehicleComponents.AsQueryable();

        if (vehicleId.HasValue)
            query = query.Where(c => c.VehicleId == vehicleId.Value);

        return await DataSourceLoader.LoadAsync(
            query.ProjectTo<VehicleComponentReadDto>(mapper.ConfigurationProvider),
            loadOptions
        );
    }

    public async Task<VehicleComponentWriteDto> GetComponentForUpdateAsync(int id)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

        return mapper.Map<VehicleComponentWriteDto>(entity);
    }

    public async Task<int> CreateComponentAsync(VehicleComponentWriteDto dto)
    {
        var entity = mapper.Map<VehicleComponent>(dto);
        context.VehicleComponents.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateComponentAsync(int id, VehicleComponentWriteDto dto)
    {
        var entity = await context.VehicleComponents.FindAsync(id)
            ?? throw new KeyNotFoundException($"VehicleComponent {id} not found.");

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

    public async Task<LoadResult> GetVehiclesLookupAsync(DataSourceLoadOptions loadOptions)
    {
        var lookup = context.Vehicles
            .OrderBy(v => v.Plate)
            .Select(v => new { Value = v.Id, Text = v.Plate });

        return await DataSourceLoader.LoadAsync(lookup, loadOptions);
    }
}