using Microsoft.EntityFrameworkCore;
using PatientService.DataAccessLayer.Data;
using PatientService.DataAccessLayer.Entities;
using PatientService.DataAccessLayer.RepositoriesContracts;

namespace PatientService.DataAccessLayer.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _db;

    public PatientRepository(PatientDbContext db)
    {
        _db = db;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
       await _db.Patients.AddAsync(patient);
       await _db.SaveChangesAsync();
       return patient;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
      var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null)
        {
            return false;
        }
        _db.Patients.Remove(patient);
       await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
      return await _db.Patients.AnyAsync(p => p.Id == id);

    }

    public async Task<List<Patient>> GetAllAsync()
    {
       return await _db.Patients.ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
       return await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
      var existingPatient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == patient.Id);
        if (existingPatient == null)
        {
            throw new KeyNotFoundException($"Patient with ID {patient.Id} was not found.");
        }
        existingPatient.FullName = patient.FullName;
        existingPatient.DateOfBirth = patient.DateOfBirth;
        existingPatient.PhoneNumber = patient.PhoneNumber;
        existingPatient.Address = patient.Address;
        existingPatient.Gender = patient.Gender;
        existingPatient.BloodGroup = patient.BloodGroup;

        existingPatient.UpdateAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return existingPatient;
    }
}
