using Microsoft.Extensions.DependencyInjection;
using PatientService.BusinessLogicLayer.Mapping;
using PatientService.BusinessLogicLayer.Services;
using PatientService.BusinessLogicLayer.ServicesContract;

namespace PatientService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientServices>();

        services.AddAutoMapper(config => { }, typeof(PatientMappingProfile));

        return services;
    }
}
