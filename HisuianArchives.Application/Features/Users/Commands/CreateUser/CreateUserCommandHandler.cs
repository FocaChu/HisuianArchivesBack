using MediatR;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Handler for the CreateUserCommand that creates a new user account and returns authentication response.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email {Email}", request.Email);

        // Check if user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User creation failed: email {Email} already exists", request.Email);
            throw new BusinessException("A user with this email already exists.");
        }

        // Hash the password
        var passwordHash = _passwordService.HashPassword(request.Password);

        // Create the new user (this will trigger the UserCreatedEvent)
        var newUser = new User(request.Name, request.Email, passwordHash, request.Bio);

        // Get the default "Customer" role
        var customerRole = await _roleRepository.GetRoleByNameAsync("Customer");
        if (customerRole == null)
        {
            _logger.LogError("Default Customer role not found");
            throw new BusinessException("System configuration error: Customer role not found.");
        }

        // Add the default role to the user
        newUser.AddRole(customerRole);

        // Persist the user (the interceptor will dispatch the UserCreatedEvent)
        await _userRepository.AddUserAsync(newUser);

        // Get the user with roles loaded for token generation
        var userWithRoles = await _userRepository.GetUserByEmailWithRolesAsync(request.Email);
        if (userWithRoles == null)
        {
            _logger.LogError("Failed to retrieve created user with email {Email}", request.Email);
            throw new BusinessException("Failed to create user account.");
        }

        // Generate JWT token
        var token = _tokenService.GenerateJwtToken(userWithRoles);

        // Map user to response DTO
        var userProfile = _mapper.Map<UserSummaryResponseDto>(userWithRoles);

        _logger.LogInformation("User created successfully with email {Email}", request.Email);

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }
}
