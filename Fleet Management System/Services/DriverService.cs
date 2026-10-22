using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Exceptions;
using Fleet_Management_System.Helpers;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class DriverService(ApplicationDbContext context, IMapper mapper) : IDriverService
{
    public async Task<IReadOnlyList<DriverReadDto>> GetDriversAsync() =>
        await context.Drivers
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ProjectTo<DriverReadDto>(mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<DriverReadDto?> GetDriverByIdAsync(int id) =>
        await context.Drivers
            .AsNoTracking()
            .Where(d => d.Id == id)
            .ProjectTo<DriverReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<DriverWriteDto> GetDriverForUpdateAsync(int id)
    {
        var entity = await context.Drivers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Driver {id} not found.");

        return mapper.Map<DriverWriteDto>(entity);
    }

    public async Task<int> CreateDriverAsync(DriverWriteDto dto)
    {
        DtoValidator.Validate(dto);

        var entity = mapper.Map<Driver>(dto);
        context.Drivers.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateDriverAsync(int id, DriverWriteDto dto)
    {
        DtoValidator.Validate(dto);

        var entity = await context.Drivers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Driver {id} not found.");

        mapper.Map(dto, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteDriverAsync(int id)
    {
        var entity = await context.Drivers
            .Include(d => d.Assignments)
            .FirstOrDefaultAsync(d => d.Id == id)
            ?? throw new KeyNotFoundException($"Driver {id} not found.");

        if (entity.Assignments.Count > 0)
        {
            throw new BusinessException(
                "Cannot delete a driver that still has vehicle assignments. Remove the assignments first.");
        }

        context.Drivers.Remove(entity);
        await context.SaveChangesAsync();
    }
}
