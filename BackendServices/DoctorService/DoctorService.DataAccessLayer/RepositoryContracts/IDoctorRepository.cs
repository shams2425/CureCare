using DoctorService.DataAccessLayer.Entities;

namespace DoctorService.DataAccessLayer.RepositoryContracts;

public interface IDoctorRepository
{
    Task<Doctor> GetByIdAsync(Guid id);
    Task<Doctor> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Doctor>> GetAllAsync();
Task<IEnumerable<Doctor>> SearchDoctorsAsync(
    string? name,
    string? specialization,
    string? city);
    // Add a new doctor profile
    Task AddAsync(Doctor doctor);

    // Update existing doctor profile details
    Task UpdateAsync(Doctor doctor);

    // Update doctor's admin approval status
    Task UpdateApprovalStatusAsync(Guid id, string status);

    // Check whether a doctor profile already exists for this user
    Task<bool> ExistsByUserIdAsync(Guid userId);
}
