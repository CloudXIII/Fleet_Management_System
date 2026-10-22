using AutoMapper;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Helpers;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DriverAssignmentService(ApplicationDbContext context, IMapper mapper) : IDriverAssignmentService
{
    public async Task<IReadOnlyList<DriverAssignmentReadDto>> GetAssignmentsAsync(int vehicleId) =>
        await context.DriverAssignments
            .AsNoTracking()
            .Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.StartDate)
            .Select(a => new DriverAssignmentReadDto
            {
                Id = a.Id,
                DriverId = a.DriverId,
                DriverName = a.Driver != null ? a.Driver.Name : string.Empty,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                VehicleId = a.VehicleId,
                IsActive = a.EndDate == null
            })
            .ToListAsync();

    public async Task<DriverAssignmentWriteDto> GetAssignmentForUpdateAsync(int id)
    {
        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        return mapper.Map<DriverAssignmentWriteDto>(entity);
    }

    public async Task<int> CreateAssignmentAsync(DriverAssignmentWriteDto dto)
    {
        DtoValidator.Validate(dto);
        await EnsureVehicleExistsAsync(dto.VehicleId);
        await EnsureDriverExistsAsync(dto.DriverId);
        await EnsureSingleActiveAssignmentAsync(dto.VehicleId, dto.EndDate);

        var entity = mapper.Map<DriverAssignment>(dto);
        context.DriverAssignments.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAssignmentAsync(int id, DriverAssignmentWriteDto dto)
    {
        DtoValidator.Validate(dto);

        var entity = await context.DriverAssignments.FindAsync(id)
            ?? throw new KeyNotFoundException($"DriverAssignment {id} not found.");

        await EnsureVehicleExistsAsync(dto.VehicleId);
        await EnsureDriverExistsAsync(dto.DriverId);
        await EnsureSingleActiveAssignmentAsync(dto.VehicleId, dto.EndDate, id);

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

    public async Task EndActiveAssignmentAsync(int vehicleId)
    {
        await EnsureVehicleExistsAsync(vehicleId);

        var active = await context.DriverAssignments
            .FirstOrDefaultAsync(a => a.VehicleId == vehicleId && a.EndDate == null)
            ?? throw new BusinessException("This vehicle has no active driver assignment to end.");

        active.EndDate = DateTime.Today;
        await context.SaveChangesAsync();
    }

    private async Task EnsureVehicleExistsAsync(int vehicleId)
    {
        var exists = await context.Vehicles.AnyAsync(v => v.Id == vehicleId);
        if (!exists)
            throw new KeyNotFoundException($"Vehicle {vehicleId} not found.");
    }

    private async Task EnsureDriverExistsAsync(int driverId)
    {
        var exists = await context.Drivers.AnyAsync(d => d.Id == driverId);
        if (!exists)
            throw new KeyNotFoundException($"Driver {driverId} not found.");
    }

    private async Task EnsureSingleActiveAssignmentAsync(int vehicleId, DateTime? endDate, int? excludeId = null)
    {
        if (endDate.HasValue)
            return;

        var hasActive = await context.DriverAssignments.AnyAsync(a =>
            a.VehicleId == vehicleId
            && a.EndDate == null
            && (!excludeId.HasValue || a.Id != excludeId.Value));

        if (hasActive)
        {
            throw new BusinessException(
                "This vehicle already has an active driver assignment. End the current one before adding another.");
        }
    }
}
