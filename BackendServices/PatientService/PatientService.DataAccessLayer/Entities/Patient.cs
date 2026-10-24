namespace PatientService.DataAccessLayer.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public GenderOptions Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public DateTime UpdateAt { get; set; }

}
