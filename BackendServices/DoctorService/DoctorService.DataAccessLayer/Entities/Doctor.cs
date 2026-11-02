namespace DoctorService.DataAccessLayer.Entities;

public class Doctor
{
    // Unique ID of the doctor's profile in DoctorService
    public Guid Id { get; set; }

    // User ID of the doctor's login account from AuthService
    public Guid UserId { get; set; }

    // Doctor's personal and contact details
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // Doctor's professional details
    public string Qualification { get; set; }
    public string Specialization { get; set; }
    public int ExperienceYears { get; set; }

    // Doctor's consultation fee and location
    public decimal ConsultationFee { get; set; }
    public string City { get; set; }

    // Optional URL of the doctor's profile image
    public string? ProfileImageUrl { get; set; }

    // Admin approval status: Pending, Approved, or Rejected
    public string ApprovalStatus { get; set; }

    // Indicates whether the doctor profile is active
    public bool IsActive { get; set; }

    // Profile creation and last update timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
