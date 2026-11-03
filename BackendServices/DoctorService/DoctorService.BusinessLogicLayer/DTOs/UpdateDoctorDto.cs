using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorService.BusinessLogicLayer.DTOs;

public class UpdateDoctorDto
{
    // Updated personal details
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // Updated professional details
    public string Qualification { get; set; }
    public string Specialization { get; set; }
    public int ExperienceYears { get; set; }

    // Updated consultation and location details
    public decimal ConsultationFee { get; set; }
    public string City { get; set; }
    public string? ProfileImageUrl { get; set; }
}
