using AutoMapper;
using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;

namespace Fleet_Management_System.Profiles;

public class VehicleComponentProfile : Profile
{
    public VehicleComponentProfile()
    {
        CreateMap<VehicleComponent, VehicleComponentReadDto>();
        CreateMap<VehicleComponent, VehicleComponentWriteDto>();
        CreateMap<VehicleComponentWriteDto, VehicleComponent>();
    }
}
