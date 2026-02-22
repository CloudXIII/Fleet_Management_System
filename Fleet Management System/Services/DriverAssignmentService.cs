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

public class DriverAssignmentService(ApplicationDbContext context, IMapper mapper) : IDriverAssignmentService
{
    public async Task<LoadResult> GetAssignmentsAsync(int? vehicleId, DataSourceLoadOptions loadOptions)
    {
        var query = context.DriverAssignments.AsQueryable();

        if (vehicleId.HasValue)
            query = query.Where(a => a.VehicleId == vehicleId.Value);

        return await DataSourceLoader.LoadAsync(
            query.ProjectTo<DriverAssignmentReadDto>(mapper.ConfigurationProvider),
            loadOptions
        );
    }

    public async Task<DriverAssignmentWriteDto> GetAssignmentForUpdateAsync(int id)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        return mapper.Map<DriverAssignmentWriteDto>(entity);
    }

    public async Task<int> CreateAssignmentAsync(DriverAssignmentWriteDto dto)
    {
        var entity = mapper.Map<DriverAssignment>(dto);
        context.DriverAssignments.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAssignmentAsync(int id, DriverAssignmentWriteDto dto)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        mapper.Map(dto, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAssignmentAsync(int id)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        context.DriverAssignments.Remove(entity);
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