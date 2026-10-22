using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IDriverService
{
    Task<IReadOnlyList<DriverReadDto>> GetDriversAsync();
    Task<DriverReadDto?> GetDriverByIdAsync(int id);
    Task<DriverWriteDto> GetDriverForUpdateAsync(int id);
    Task<int> CreateDriverAsync(DriverWriteDto dto);
    Task UpdateDriverAsync(int id, DriverWriteDto dto);
    Task DeleteDriverAsync(int id);
}
