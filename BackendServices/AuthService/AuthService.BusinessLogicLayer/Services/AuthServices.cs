using AuthService.BusinessLogicLayer.DTOs;
using AuthService.BusinessLogicLayer.ServiceContracts;
using AuthService.DataAccessLayer.Entities;
using AuthService.DataAccessLayer.RepositoriesContracts;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace AuthService.BusinessLogicLayer.Services;

public class AuthServices : IAuthServices
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _password;
    private readonly IMapper _mapper;

    public AuthServices(IUserRepository userRepository, IPasswordHasher<User> password, IMapper mapper)
    {
        _userRepository = userRepository;
        _password = password;
        _mapper = mapper;
    }

    public async Task LoginAsync(LoginRequestDto loginRequestDto)
    { 
        User? user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);

        // Reject the login if the email is not registered.
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Compare the entered password with the stored hash.
        PasswordVerificationResult result = _password.VerifyHashedPassword(user,user.PasswordHash,loginRequestDto.Password);

        // Reject the login if the password is incorrect.
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
    }

    public async Task RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        User? existingUser =await _userRepository.GetByEmailAsync(registerRequestDto.Email);
       

        if(existingUser != null)
        {
            throw new InvalidOperationException("An account with this email is already exist");
        }

        var user = _mapper.Map<User>(registerRequestDto);

        // Step 3: Hash the password before saving it.
        user.PasswordHash = _password.HashPassword(user,registerRequestDto.Password);

        await _userRepository.AddAsync(user);
    }
}
