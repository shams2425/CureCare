using AuthService.DataAccessLayer.Data;
using AuthService.DataAccessLayer.Repositories;
using AuthService.DataAccessLayer.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.DataAccessLayer
{
    // Keeps all DAL service registrations in one place.
    public static class DependencyInjection
    {
        // Registers database and repository services in the DI container.
        public static IServiceCollection AddDataAccessLayer( this IServiceCollection services, IConfiguration configuration)
        {
            // Configures EF Core to use SQL Server.
            string connectionString = configuration.GetConnectionString("AuthDb")!;
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            // Registers the repository contract and its implementation.
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}