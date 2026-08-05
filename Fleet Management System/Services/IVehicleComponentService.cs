using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IVehicleComponentService
{
    Task<IReadOnlyList<VehicleComponentReadDto>> GetComponentsAsync(int? vehicleId);
    Task<VehicleComponentWriteDto> GetComponentForUpdateAsync(int id);
    Task<VehicleComponentReadDto> CreateComponentAsync(VehicleComponentWriteDto dto);
    Task<VehicleComponentReadDto> UpdateComponentAsync(int id, VehicleComponentWriteDto dto);
    Task DeleteComponentAsync(int id);
}
