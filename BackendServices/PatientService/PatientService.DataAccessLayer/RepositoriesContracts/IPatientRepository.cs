using PatientService.DataAccessLayer.Entities;

namespace PatientService.DataAccessLayer.RepositoriesContracts;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id);
    Task<List<Patient>> GetAllAsync();
    Task<Patient> AddAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
