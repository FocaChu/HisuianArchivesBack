using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.Exceptions;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
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

}