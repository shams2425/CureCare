using Microsoft.EntityFrameworkCore;
using PatientService.DataAccessLayer.Entities;

namespace PatientService.DataAccessLayer.Data;

public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
    {
    }
    public DbSet<Patient> Patients { get; set; }
}
