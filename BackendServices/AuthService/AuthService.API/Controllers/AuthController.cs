using AuthService.BusinessLogicLayer.DTOs;
using AuthService.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _services;

    public AuthController(IAuthServices services)
    {
        _services = services;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
    {
        await _services.RegisterAsync(registerRequestDto);
        return Ok(new
        {
            Message = "User Register SuccessFully."
        });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        await _services.LoginAsync(loginRequestDto);
        return Ok(new
        {
            Message = "User Login Successfully"
        });
    }
}
