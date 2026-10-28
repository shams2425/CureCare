
using AuthService.BusinessLogicLayer.DTOs;
using AuthService.BusinessLogicLayer.ServiceContracts;
using AuthService.DataAccessLayer.Entities;
using AuthService.DataAccessLayer.RepositoriesContracts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthService.BusinessLogicLayer.Services;

public class AuthServices : IAuthServices
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _password;
    private readonly IMapper _mapper;

    public AuthServices(
        IUserRepository userRepository,
        IPasswordHasher<User> password,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _password = password;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto loginRequestDto)
    {
        // Find the user by email.
        User? user = await _userRepository
            .GetByEmailAsync(loginRequestDto.Email);

        // Reject login if the email is not registered.
        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password");
        }

        // Verify the entered password against the stored hash.
        PasswordVerificationResult result =
            _password.VerifyHashedPassword(
                user,
                user.PasswordHash!,
                loginRequestDto.Password!);

        // Reject login if the password is incorrect.
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password");
        }

        // Return user details after successful login.
        return new LoginResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task RegisterAdminAsync(RegisterRequestDto registerRequestDto)
    {
        User? existingUser = await _userRepository.GetByEmailAsync(registerRequestDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("An account with this email is already exists.");
        }
        User user = _mapper.Map<User>(registerRequestDto);
        user.Role = "Admin";

        user.PasswordHash = _password.HashPassword(user, registerRequestDto.Password!);
        await _userRepository.AddAsync(user);
    }

    public async Task RegisterAsync(
        RegisterRequestDto registerRequestDto)
    {
        // Check whether this email is already registered.
        User? existingUser = await _userRepository
            .GetByEmailAsync(registerRequestDto.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        // Convert registration DTO into the User entity.
        var user = _mapper.Map<User>(registerRequestDto);

        // Public registration must never assign the Admin role.
        user.Role = "User";

        // Hash the password before saving it.
        user.PasswordHash = _password.HashPassword(
            user,
            registerRequestDto.Password!);

        await _userRepository.AddAsync(user);
    }
}
