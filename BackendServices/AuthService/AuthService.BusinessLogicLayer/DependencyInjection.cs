using AuthService.BusinessLogicLayer.Mappings;
using AuthService.BusinessLogicLayer.ServiceContracts;
using AuthService.BusinessLogicLayer.Services;
using AuthService.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddAutoMapper(
        cfg => { },
        typeof(AuthMappingProfile).Assembly);
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }
}
