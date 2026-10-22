using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DriverAssignmentService(ApplicationDbContext context, IMapper mapper) : IDriverAssignmentService
{
    public async Task<IReadOnlyList<DriverAssignmentReadDto>> GetAssignmentsAsync(int vehicleId) =>
        await context.DriverAssignments
            .Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.StartDate)
            .ProjectTo<DriverAssignmentReadDto>(mapper.ConfigurationProvider)
            .ToListAsync();

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
}
