using AutoMapper;
using DoctorService.BusinessLogicLayer.DTOs;
using DoctorService.DataAccessLayer.Entities;
using System.Numerics;

namespace DoctorService.BusinessLogicLayer.Mappers;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        // Map doctor registration DTO to database entity
        CreateMap<CreateDoctorDto, Doctor>();

        // Map doctor update DTO to database entity
        CreateMap<UpdateDoctorDto, Doctor>();

        // Map database entity to API response DTO
        CreateMap<Doctor, DoctorResponseDto>();
    }
}
