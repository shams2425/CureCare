using DoctorService.BusinessLogicLayer.DTOs;

namespace DoctorService.BusinessLogicLayer.ServiceContracts
{
    public interface IDoctorService
    {
        // Register a new doctor profile
        Task<DoctorResponseDto> RegisterDoctorAsync(CreateDoctorDto doctorDto);

        // Get doctor profile by Doctor ID
        Task<DoctorResponseDto?> GetDoctorByIdAsync(Guid id);

        // Get doctor profile using AuthService User ID
        Task<DoctorResponseDto?> GetDoctorByUserIdAsync(Guid userId);

        // Get all doctor profiles
        Task<IEnumerable<DoctorResponseDto>> GetAllDoctorsAsync();

        // Search approved doctors by name, specialization, and city
        Task<IEnumerable<DoctorResponseDto>> SearchDoctorsAsync( string? name,string? specialization, string? city);

        // Update doctor profile
        Task<DoctorResponseDto?> UpdateDoctorAsync(
            Guid id, UpdateDoctorDto doctorDto);

        // Approve or reject doctor registration (Admin operation)
        Task<bool> UpdateApprovalStatusAsync(Guid id, string status); 
    }
}