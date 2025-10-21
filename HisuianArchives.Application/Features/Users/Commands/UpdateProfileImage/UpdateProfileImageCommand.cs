using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfileImage;

/// <summary>
/// Command for updating a user's profile image.
/// </summary>
public class UpdateProfileImageCommand : IRequest<UserSummaryResponseDto>
{
    public Guid UserId { get; set; }
    public Guid ImageId { get; set; }
}
