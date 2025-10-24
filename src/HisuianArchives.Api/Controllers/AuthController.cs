using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.Features.Users.Commands.CreateUser;
using HisuianArchives.Application.Features.Users.Commands.Login;
using Microsoft.AspNetCore.Mvc;

namespace HisuianArchives.Api.Controllers;

/// <summary>
/// Controller responsible for user authentication operations such as registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="logger">The logger instance.</param>
    public AuthController(IMediator mediator, IMapper mapper, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user and returns authentication information.
    /// </summary>
    /// <param name="registerDto">The registration request data transfer object.</param>
    /// <returns>The authentication response containing the JWT token and user profile.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]      
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        _logger.LogInformation("Attempting to register new user with email: {Email}", registerDto.Email);

        // Map the DTO to the command
        var command = _mapper.Map<CreateUserCommand>(registerDto);

        // Send the command through MediatR
        var authResponse = await _mediator.Send(command);

        _logger.LogInformation("User {Email} registered and logged in successfully.", registerDto.Email);

        return Created($"/api/users/{authResponse.UserProfile.UserId}", authResponse);
    }

    /// <summary>
    /// Authenticates a user and returns authentication information.
    /// </summary>
    /// <param name="loginDto">The login request data transfer object.</param>
    /// <returns>The authentication response containing the JWT token and user profile.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        _logger.LogInformation("User with email {Email} is attempting to log in.", loginDto.Email);

        // Map the DTO to the command
        var command = _mapper.Map<LoginCommand>(loginDto);

        // Send the command through MediatR
        var authResponse = await _mediator.Send(command);

        _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
        return Ok(authResponse);
    }
}
