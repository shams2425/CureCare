using DoctorService.DataAccessLayer.Data;
using DoctorService.DataAccessLayer.Repositories;
using DoctorService.DataAccessLayer.RepositoryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorService.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DapperDbContext>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        return services;
    }
}
