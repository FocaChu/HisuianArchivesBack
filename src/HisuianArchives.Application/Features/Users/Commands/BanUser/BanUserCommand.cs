using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Commands.BanUser;

/// <summary>
/// Command for banning a user account.
/// </summary>
public class BanUserCommand : IRequest<UserSummaryResponseDto>
{
    public Guid UserId { get; set; }

    public string Reason { get; set; } = null!;
    
    public DateTimeOffset? BannedUntil { get; set; }
}
