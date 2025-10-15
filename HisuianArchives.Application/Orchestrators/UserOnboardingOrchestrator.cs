using AutoMapper;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Services;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Orchestrators;

public class UserOnboardingOrchestrator : IUserOnboardingOrchestrator
{
    private readonly IUserService _userService;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UserOnboardingOrchestrator(
        IUserService userService,
        IRoleRepository roleRepository,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userService = userService;
        _roleRepository = roleRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto registerDto)
    {
        // Get default "Customer" role
        var defaultRole = await _roleRepository.GetByNameAsync("Customer");
        if (defaultRole == null)
        {
            throw new InvalidOperationException("Default Customer role not found in the system.");
        }

        // Create user with initial role
        var newUser = await _userService.CreateUserAsync(
            registerDto.Name,
            registerDto.Email,
            registerDto.Password,
            registerDto.Bio,
            defaultRole);

        // Generate JWT token
        var token = _tokenService.GenerateJwtToken(newUser);

        // Build user profile
        var userProfile = _mapper.Map<UserSummaryResponseDto>(newUser);

        // Return authentication response
        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }
}
