namespace DoctorService.BusinessLogicLayer.DTOs
{
    public class DoctorResponseDto
    {
        // Doctor profile ID
        public Guid Id { get; set; }

        // AuthService account ID of the doctor
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

        // Admin approval and account status
        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }

        // Profile timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}