using System.ComponentModel.DataAnnotations;

namespace CureCare.Web.Models;

public class UpdatePatientViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Patient name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Please select a gender.")]
    public string Gender { get; set; } = string.Empty;

    [StringLength(20)]
    public string? BloodGroup { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(15, MinimumLength = 10,
        ErrorMessage = "Phone number must be between 10 and 15 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Address { get; set; }
}