using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Commands.UnbanUser;

/// <summary>
/// Command for unbanning a user account.
/// </summary>
public class UnbanUserCommand : IRequest<UserSummaryResponseDto>
{
    public Guid UserId { get; set; }
}
