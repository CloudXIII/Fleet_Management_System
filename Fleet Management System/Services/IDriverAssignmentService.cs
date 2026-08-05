using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface IDriverAssignmentService
{
    Task<IReadOnlyList<DriverAssignmentReadDto>> GetAssignmentsAsync(int? vehicleId);
    Task<DriverAssignmentWriteDto> GetAssignmentForUpdateAsync(int id);
    Task<DriverAssignmentReadDto> CreateAssignmentAsync(DriverAssignmentWriteDto dto);
    Task<DriverAssignmentReadDto> UpdateAssignmentAsync(int id, DriverAssignmentWriteDto dto);
    Task DeleteAssignmentAsync(int id);
}
