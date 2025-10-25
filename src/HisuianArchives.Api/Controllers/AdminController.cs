using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Features.Users.Commands.BanUser;
using HisuianArchives.Application.Features.Users.Commands.UnbanUser;
using HisuianArchives.Application.Features.Users.Queries.GetUserForAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HisuianArchives.Api.Controllers;

/// <summary>
/// Controller responsible for administrative operations such as banning and unbanning users.
/// </summary>
[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="logger">The logger instance.</param>
    public AdminController(IMediator mediator, IMapper mapper, ILogger<AdminController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Bans a user account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to ban.</param>
    /// <param name="banRequest">The ban request data transfer object.</param>
    /// <returns>The updated user profile.</returns>
    [HttpPost("users/{userId}/ban")]
    [ProducesResponseType(typeof(UserSummaryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BanUser(Guid userId, [FromBody] BanUserRequestDto banRequest)
    {
        _logger.LogInformation("Admin attempting to ban user {UserId} with reason: {Reason}", userId, banRequest.Reason);

        // Map the DTO to the command and set the user ID
        var command = _mapper.Map<BanUserCommand>(banRequest);
        command.UserId = userId;

        // Send the command through MediatR
        var responseDto = await _mediator.Send(command);

        _logger.LogInformation("User {UserId} banned successfully by admin", userId);

        return Ok(responseDto);
    }

    /// <summary>
    /// Unbans a user account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to unban.</param>
    /// <returns>The updated user profile.</returns>
    [HttpPost("users/{userId}/unban")]
    [ProducesResponseType(typeof(UserSummaryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnbanUser(Guid userId)
    {
        _logger.LogInformation("Admin attempting to unban user {UserId}", userId);

        // Create the command
        var command = new UnbanUserCommand { UserId = userId };

        // Send the command through MediatR
        var responseDto = await _mediator.Send(command);

        _logger.LogInformation("User {UserId} unbanned successfully by admin", userId);

        return Ok(responseDto);
    }

    /// <summary>
    /// Searches for a user by ID or email for admin purposes.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to search for.</param>
    /// <param name="email">The email address of the user to search for.</param>
    /// <returns>The user profile with admin information.</returns>
    [HttpGet("users/search")]
    [ProducesResponseType(typeof(AdminUserSummaryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchUser([FromQuery] Guid? userId, [FromQuery] string? email)
    {
        _logger.LogInformation("Admin searching for user - UserId: {UserId}, Email: {Email}", userId, email);

        // Create the query
        var query = new GetUserForAdminQuery 
        { 
            UserId = userId, 
            Email = email 
        };

        // Send the query through MediatR
        var responseDto = await _mediator.Send(query);

        _logger.LogInformation("User found successfully for admin - UserId: {UserId}", responseDto.UserId);

        return Ok(responseDto);
    }
}
