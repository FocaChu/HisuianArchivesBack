using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfile;

/// <summary>
/// Command for updating a user's profile information.
/// </summary>
public class UpdateProfileCommand : IRequest<UserSummaryResponseDto>
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Bio { get; set; }
}
