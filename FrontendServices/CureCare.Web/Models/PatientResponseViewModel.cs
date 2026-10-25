namespace CureCare.Web.Models;

public class PatientResponseViewModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string? BloodGroup { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
