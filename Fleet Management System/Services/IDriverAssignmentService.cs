using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IDriverAssignmentService
{
    Task<LoadResult> GetAssignmentsAsync(int? vehicleId, DataSourceLoadOptions loadOptions);
    Task<DriverAssignmentWriteDto> GetAssignmentForUpdateAsync(int id);
    Task<int> CreateAssignmentAsync(DriverAssignmentWriteDto dto);
    Task UpdateAssignmentAsync(int id, DriverAssignmentWriteDto dto);
    Task DeleteAssignmentAsync(int id);
    Task<LoadResult> GetVehiclesLookupAsync(DataSourceLoadOptions loadOptions);
}