using AuthService.BusinessLogicLayer.DTOs;

namespace AuthService.BusinessLogicLayer.ServiceContracts;

public interface IAuthServices
{
    Task RegisterAsync(RegisterRequestDto registerRequestDto);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task RegisterAdminAsync(RegisterRequestDto registerRequestDto);
}
