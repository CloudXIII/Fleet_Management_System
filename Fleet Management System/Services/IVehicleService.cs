using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services
{
    public interface IVehicleService
    {
        Task<LoadResult> GetVehiclesAsync(DataSourceLoadOptions loadOptions);
        Task<VehicleReadDto?> GetVehicleByIdAsync(int id);
        Task<VehicleWriteDto> GetVehicleForUpdateAsync(int id);
        Task<int> CreateVehicleAsync(VehicleWriteDto dto);
        Task UpdateVehicleAsync(int id, VehicleWriteDto dto);
        Task DeleteVehicleAsync(int key);
    }
}