using DoctorService.BusinessLogicLayer.Mappers;
using DoctorService.BusinessLogicLayer.ServiceContracts;
using DoctorService.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DoctorMappingProfile).Assembly);
        services.AddScoped<IDoctorService,DoctorServices>();
        return services;
    }
}
