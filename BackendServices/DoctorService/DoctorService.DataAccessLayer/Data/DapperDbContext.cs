using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DoctorService.DataAccessLayer.Data;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
      var connectionString =  _configuration.GetConnectionString("DoctorDb");
        return new SqlConnection(connectionString);
    }
}
