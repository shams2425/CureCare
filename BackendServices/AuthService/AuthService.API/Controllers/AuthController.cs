
using AuthService.BusinessLogicLayer.DTOs;
using AuthService.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _services;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthServices services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    // Register a new user account.
    [HttpPost("Register")]
    public async Task<IActionResult> Register( RegisterRequestDto registerRequestDto)
    {
        await _services.RegisterAsync(registerRequestDto);

        return Ok(new
        {
            Message = "User Registered Successfully."
        });
    }

    // Login and return the user's details and role.
    [HttpPost("Login")]
    public async Task<IActionResult> Login( LoginRequestDto loginRequestDto)
    {
        LoginResponseDto response = await _services.LoginAsync(loginRequestDto);

        return Ok(response);
    }

    [HttpPost("RegisterAdmin")]
    public async Task<IActionResult> RegisterAdmin(RegisterRequestDto registerRequestDto)
    {
        var configuredSecret = _configuration["AdminSetup:Secret"];
        if (string.IsNullOrWhiteSpace(configuredSecret))
        {
            return StatusCode(503, new
            {
                Message = "Admin registered is not configured"
            });
        }
        if (!Request.Headers.TryGetValue("X-Admin-Setup-Secret",out var suppliedSecret)|| !string.Equals(configuredSecret,suppliedSecret.ToString(),
                      StringComparison.Ordinal))
        {
            return Unauthorized(new
            {
                Message = "Invalid setup secret."
            });
        }

        await _services.RegisterAdminAsync(registerRequestDto);
        return Ok(new
        {
            Message = "Admin Registered Successfully."
        });
    }
}
