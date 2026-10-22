using AutoMapper;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Helpers;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class VehicleService(ApplicationDbContext context, IMapper mapper) : IVehicleService
{
    private static readonly TimeSpan InsuranceWarningWindow = TimeSpan.FromDays(30);

    public async Task<IReadOnlyList<VehicleReadDto>> GetVehiclesAsync() =>
        await ProjectToReadDto(context.Vehicles.AsNoTracking().OrderBy(v => v.Id))
            .ToListAsync();

    public async Task<VehicleReadDto?> GetVehicleByIdAsync(int id) =>
        await ProjectToReadDto(context.Vehicles.AsNoTracking().Where(v => v.Id == id))
            .FirstOrDefaultAsync();

    public async Task<VehicleWriteDto> GetVehicleForUpdateAsync(int id)
    {
        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        return mapper.Map<VehicleWriteDto>(entity);
    }

    public async Task<int> CreateVehicleAsync(VehicleWriteDto dto)
    {
        DtoValidator.Validate(dto);
        dto.Plate = dto.Plate.Trim();
        await EnsureUniquePlateAsync(dto.Plate);

        var entity = mapper.Map<Vehicle>(dto);
        context.Vehicles.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateVehicleAsync(int id, VehicleWriteDto dto)
    {
        DtoValidator.Validate(dto);
        dto.Plate = dto.Plate.Trim();

        var entity = await context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Vehicle {id} not found.");

        await EnsureUniquePlateAsync(dto.Plate, id);
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

    private static IQueryable<VehicleReadDto> ProjectToReadDto(IQueryable<Vehicle> query)
    {
        var warningDate = DateTime.Today.Add(InsuranceWarningWindow);

        return query.Select(v => new VehicleReadDto
        {
            Id = v.Id,
            Plate = v.Plate,
            Brand = v.Brand,
            Model = v.Model,
            InsuranceExpiry = v.InsuranceExpiry,
            HasActiveDriver = v.Assignments.Any(a => a.EndDate == null),
            IsInsuranceExpiringSoon = v.InsuranceExpiry <= warningDate
        });
    }

    private async Task EnsureUniquePlateAsync(string plate, int? excludeId = null)
    {
        var normalized = plate.Trim();
        var exists = await context.Vehicles.AnyAsync(v =>
            v.Plate == normalized && (!excludeId.HasValue || v.Id != excludeId.Value));

        if (exists)
            throw new BusinessException($"A vehicle with plate '{normalized}' already exists.");
    }
}
