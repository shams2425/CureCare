using System.ComponentModel.DataAnnotations;

namespace AuthService.BusinessLogicLayer.DTOs;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Full name must be between 2 and 100 characters.")]
    public string? FullName { get; set; }


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters long.")]
    public string? Password { get; set; }
}
