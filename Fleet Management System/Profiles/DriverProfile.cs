using AutoMapper;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;

namespace Fleet_Management_System.Profiles;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<Driver, DriverReadDto>();
        CreateMap<Driver, DriverWriteDto>();
        CreateMap<DriverWriteDto, Driver>();
    }
}
