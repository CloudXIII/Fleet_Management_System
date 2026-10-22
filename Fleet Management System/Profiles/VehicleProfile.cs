using AutoMapper;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;

namespace Fleet_Management_System.Profiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<VehicleWriteDto, Vehicle>();
        CreateMap<Vehicle, VehicleWriteDto>();
    }
}
