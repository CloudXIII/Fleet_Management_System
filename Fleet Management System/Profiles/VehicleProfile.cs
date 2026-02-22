using AutoMapper;
using Fleet_Management_System.Models;
using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Profiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleReadDto>();
            CreateMap<VehicleWriteDto, Vehicle>();
            CreateMap<Vehicle, VehicleWriteDto>();
        }
    }
}