using AutoMapper;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Exceptions;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailWithRolesAsync(loginDto.Email);

        if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new BusinessException("Invalid email or password.");
        }

        var token = _tokenService.GenerateJwtToken(user);

        var userProfile = _mapper.Map<UserSummaryResponseDto>(user);

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }

}