namespace DoctorService.BusinessLogicLayer.DTOs;

public class CreateDoctorDto
{
    // Doctor's AuthService account ID
    public Guid UserId { get; set; }

    // Personal details
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // Professional details
    public string Qualification { get; set; }
    public string Specialization { get; set; }
    public int ExperienceYears { get; set; }

    // Consultation and location details
    public decimal ConsultationFee { get; set; }
    public string City { get; set; }
    public string? ProfileImageUrl { get; set; }
}
