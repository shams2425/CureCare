using PatientService.DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PatientService.BusinessLogicLayer.DTOs;

public class PatientRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string? FullName { get; set; }

    [Required]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    public GenderOptions Gender { get; set; }

    [StringLength(20)]
    public string? BloodGroup { get; set; }

    [Required]
    [Phone]
    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }
}
