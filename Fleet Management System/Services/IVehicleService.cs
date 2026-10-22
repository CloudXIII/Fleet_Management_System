using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IVehicleService
{
    Task<IReadOnlyList<VehicleReadDto>> GetVehiclesAsync();
    Task<VehicleReadDto?> GetVehicleByIdAsync(int id);
    Task<VehicleWriteDto> GetVehicleForUpdateAsync(int id);
    Task<int> CreateVehicleAsync(VehicleWriteDto dto);
    Task UpdateVehicleAsync(int id, VehicleWriteDto dto);
    Task DeleteVehicleAsync(int id);
}
