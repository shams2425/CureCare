using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatientService.DataAccessLayer.Data;
using PatientService.DataAccessLayer.RepositoriesContracts;
using PatientService.DataAccessLayer.Repository;

namespace PatientService.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLAyer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        string connectionString = configuration.GetConnectionString("PatientDb")!;
        services.AddDbContext<PatientDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
        return services;
    }
}
