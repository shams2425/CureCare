using AutoMapper;
using PatientService.BusinessLogicLayer.DTOs;
using PatientService.DataAccessLayer.Entities;

namespace PatientService.BusinessLogicLayer.Mapping;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        // Create request se Patient entity banegi.
        CreateMap<PatientRequest, Patient>();

        // Update request se Patient entity banegi.
        CreateMap<UpdatePatientRequest, Patient>();

        // Patient entity se API response DTO banega.
        CreateMap<Patient, PatientResponse>()
            .ForMember(destination => destination.Gender,options => options.MapFrom(source => source.Gender.ToString()));
    }
}