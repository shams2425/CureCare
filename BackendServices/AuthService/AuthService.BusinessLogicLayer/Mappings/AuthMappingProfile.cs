using AuthService.BusinessLogicLayer.DTOs;
using AuthService.DataAccessLayer.Entities;
using AutoMapper;

namespace AuthService.BusinessLogicLayer.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
