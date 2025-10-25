using MediatR;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.UnbanUser;

/// <summary>
/// Handler for the UnbanUserCommand that unbans a user account.
/// </summary>
public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand, UserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UnbanUserCommandHandler> _logger;

    public UnbanUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UnbanUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserSummaryResponseDto> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unbanning user {UserId}", request.UserId);

        // Get the user
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("Unban failed: user {UserId} not found", request.UserId);
            throw new BusinessException("User not found.");
        }

        // Unban the user
        user.Unban();

        // Persist the changes
        await _userRepository.UpdateAsync(user);

        // Map to response DTO
        var responseDto = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("User {UserId} unbanned successfully", request.UserId);

        return responseDto;
    }
}
