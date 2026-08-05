using AutoMapper;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DriverAssignmentService(ApplicationDbContext context, IMapper mapper) : IDriverAssignmentService
{
    public async Task<IReadOnlyList<DriverAssignmentReadDto>> GetAssignmentsAsync(int? vehicleId)
    {
        var query = context.DriverAssignments.AsNoTracking().AsQueryable();

        if (vehicleId.HasValue)
            query = query.Where(a => a.VehicleId == vehicleId.Value);

        var assignments = await query
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();

        return assignments.Select(MapAssignment).ToList();
    }

    public async Task<DriverAssignmentWriteDto> GetAssignmentForUpdateAsync(int id)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        return mapper.Map<DriverAssignmentWriteDto>(entity);
    }

    public async Task<DriverAssignmentReadDto> CreateAssignmentAsync(DriverAssignmentWriteDto dto)
    {
        await EnsureVehicleExists(dto.VehicleId);
        ValidateDates(dto);

        var entity = mapper.Map<DriverAssignment>(dto);
        context.DriverAssignments.Add(entity);
        await context.SaveChangesAsync();

        return MapAssignment(entity);
    }

    public async Task<DriverAssignmentReadDto> UpdateAssignmentAsync(int id, DriverAssignmentWriteDto dto)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        await EnsureVehicleExists(dto.VehicleId);
        ValidateDates(dto);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync();

        return MapAssignment(entity);
    }

    public async Task DeleteAssignmentAsync(int id)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        context.DriverAssignments.Remove(entity);
        await context.SaveChangesAsync();
    }

    private async Task EnsureVehicleExists(int vehicleId)
    {
        if (!await context.Vehicles.AnyAsync(v => v.Id == vehicleId))
            throw new KeyNotFoundException($"Vehicle {vehicleId} not found.");
    }

    private static void ValidateDates(DriverAssignmentWriteDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value.Date < dto.StartDate.Date)
            throw new ArgumentException("End date cannot be earlier than start date.");
    }

    private static DriverAssignmentReadDto MapAssignment(DriverAssignment assignment) =>
        new()
        {
            Id = assignment.Id,
            DriverName = assignment.DriverName,
            StartDate = assignment.StartDate,
            EndDate = assignment.EndDate,
            VehicleId = assignment.VehicleId,
            IsActive = !assignment.EndDate.HasValue || assignment.EndDate > DateTime.UtcNow
        };
}
