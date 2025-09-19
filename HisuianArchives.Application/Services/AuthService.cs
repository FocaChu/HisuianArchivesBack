using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.Exceptions;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository; 
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailWithRolesAsync(loginDto.Email);

        if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new BusinessException("Invalid email or password.");
        }

        var token = _tokenService.GenerateJwtToken(user);

        var userProfile = new UserSummaryResponseDto
        {
            UserId = user.Id,
            Bio = user.Bio,
            Name = user.Name,
            Email = user.Email
        };

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new BusinessException("A user with this email already exists.");
        }

        var passwordHash = _passwordService.HashPassword(registerDto.Password);
        var newUser = new User(registerDto.Name, registerDto.Email, passwordHash, registerDto.Bio);

        var defaultRole = await _roleRepository.GetByNameAsync("Customer");
        if (defaultRole != null)
        {
            newUser.Roles.Add(defaultRole);
        }

        await _userRepository.AddUserAsync(newUser);

        var token = _tokenService.GenerateJwtToken(newUser);

        var userProfile = new UserSummaryResponseDto
        {
            UserId = newUser.Id,
            Bio = newUser.Bio,
            Name = newUser.Name,
            Email = newUser.Email
        };

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }
}