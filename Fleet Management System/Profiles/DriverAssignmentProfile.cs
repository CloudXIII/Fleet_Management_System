using AutoMapper;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;

namespace Fleet_Management_System.Profiles;

public class DriverAssignmentProfile : Profile
{
    public DriverAssignmentProfile()
    {
        CreateMap<DriverAssignment, DriverAssignmentWriteDto>();
        CreateMap<DriverAssignmentWriteDto, DriverAssignment>();
    }
}
