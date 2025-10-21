using MediatR;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.Login;

/// <summary>
/// Handler for the LoginCommand that authenticates a user and returns authentication response.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<LoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting login for user with email {Email}", request.Email);

        // Get user with roles for authentication
        var user = await _userRepository.GetUserByEmailWithRolesAsync(request.Email);

        if (user == null)
        {
            _logger.LogWarning("Login failed: user with email {Email} not found", request.Email);
            throw new BusinessException("Invalid email or password.");
        }

        // Verify password
        if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: invalid password for user {Email}", request.Email);
            throw new BusinessException("Invalid email or password.");
        }

        // Generate JWT token
        var token = _tokenService.GenerateJwtToken(user);

        // Map user to response DTO
        var userProfile = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("User {Email} logged in successfully", request.Email);

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }
}
