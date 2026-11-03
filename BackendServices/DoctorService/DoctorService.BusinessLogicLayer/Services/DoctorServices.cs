using AutoMapper;
using DoctorService.BusinessLogicLayer.DTOs;
using DoctorService.BusinessLogicLayer.ServiceContracts;
using DoctorService.DataAccessLayer.Entities;
using DoctorService.DataAccessLayer.Repositories;
using DoctorService.DataAccessLayer.RepositoryContracts;

namespace DoctorService.BusinessLogicLayer.Services;

public class DoctorServices : IDoctorService
{
    private readonly IDoctorRepository _repo;
    private readonly IMapper _mapper;

    public DoctorServices(IDoctorRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // Get all doctor profiles
    public async Task<IEnumerable<DoctorResponseDto>> GetAllDoctorsAsync()
    {
      var doctor = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorResponseDto>>(doctor);
    }

    // Get doctor profile by Doctor ID
    public async Task<DoctorResponseDto?> GetDoctorByIdAsync(Guid id)
    {
      var doctor =  await _repo.GetByIdAsync(id);
        if (doctor == null)
        {
            return null;
        }
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    // Get doctor profile using AuthService User ID
    public async Task<DoctorResponseDto?> GetDoctorByUserIdAsync(Guid userId)
    {
      var doctor = await _repo.GetByUserIdAsync(userId);
        if (doctor == null)
        {
            return null;
        }
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    // Register a new doctor profile
    public async Task<DoctorResponseDto> RegisterDoctorAsync(CreateDoctorDto doctorDto)
    {
        // check wheter the AuthService UserId is valid
        if (doctorDto.UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is invalid");
        }

        // Prevent duplicate doctor profiles
       if( await _repo.ExistsByUserIdAsync(doctorDto.UserId))
        {
            throw new ArgumentException("Doctor profile is already exists.");
        }
       Doctor doctor = _mapper.Map<Doctor>(doctorDto);

        doctor.Id = Guid.NewGuid();
        doctor.ApprovalStatus = "Pending";
        doctor.IsActive = true;
        doctor.CreatedAt = DateTime.UtcNow;

        await _repo.AddAsync(doctor);

        // Convert entity into response DTO
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    // Search approved and active doctors
    public async Task<IEnumerable<DoctorResponseDto>> SearchDoctorsAsync(string? name, string? specialization, string? city)
    {
        // Check whether at least one search filter is provided
        if (string.IsNullOrWhiteSpace(name) &&
            string.IsNullOrWhiteSpace(specialization) &&
            string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException(
                "Please provide at least one search filter.");
        }

        // Search doctors from database
        var doctors = await _repo.SearchDoctorsAsync(name, specialization, city);

        // Convert doctor entities into response DTOs
        return _mapper.Map<IEnumerable<DoctorResponseDto>>(doctors);
    }

    // Admin operation: approve or reject doctor registration
    public async Task<bool> UpdateApprovalStatusAsync(Guid id, string status)
    {
        // validate approval status
        if(status != "Approved" && status != "Reject" && status != "Pending")
        {
            throw new ArgumentException("Invalid approval status.");
        }
        var doctor = await _repo.GetByIdAsync(id);

        if (doctor == null)
        {
            return false;
        }
        await _repo.UpdateApprovalStatusAsync(id, status);
        return true;
    }

    // Update existing doctor profile
    public async Task<DoctorResponseDto?> UpdateDoctorAsync(Guid id, UpdateDoctorDto doctorDto)
    {
        var doctor = await _repo.GetByIdAsync(id);
        if (doctor == null)
        {
            return null;
        }
        _mapper.Map(doctorDto,doctor);

        doctor.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(doctor);
        return _mapper.Map<DoctorResponseDto>(doctor);
    }
}
