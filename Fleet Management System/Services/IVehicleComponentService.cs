using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services
{
    public interface IVehicleComponentService
    {
        Task<LoadResult> GetComponentsAsync(int? vehicleId, DataSourceLoadOptions loadOptions);
        Task<VehicleComponentWriteDto> GetComponentForUpdateAsync(int id);
        Task<int> CreateComponentAsync(VehicleComponentWriteDto dto);
        Task UpdateComponentAsync(int id, VehicleComponentWriteDto dto);
        Task DeleteComponentAsync(int id);
        Task<LoadResult> GetVehiclesLookupAsync(DataSourceLoadOptions loadOptions);
    }
}
