using MediatR;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.BanUser;

/// <summary>
/// Handler for the BanUserCommand that bans a user account.
/// </summary>
public class BanUserCommandHandler : IRequestHandler<BanUserCommand, UserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BanUserCommandHandler> _logger;

    public BanUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<BanUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserSummaryResponseDto> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Banning user {UserId} with reason: {Reason}", request.UserId, request.Reason);

        // Get the user
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("Ban failed: user {UserId} not found", request.UserId);
            throw new BusinessException("User not found.");
        }

        // Ban the user
        user.Ban(request.Reason, request.BannedUntil);

        // Persist the changes
        await _userRepository.UpdateAsync(user);

        // Map to response DTO
        var responseDto = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("User {UserId} banned successfully until {BannedUntil}", 
            request.UserId, request.BannedUntil?.ToString("yyyy-MM-dd HH:mm:ss") ?? "permanently");

        return responseDto;
    }
}
