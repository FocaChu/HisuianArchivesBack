using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Queries.GetUserForAdmin;

/// <summary>
/// Query for retrieving a user by ID or email for admin purposes.
/// </summary>
public class GetUserForAdminQuery : IRequest<AdminUserSummaryResponseDto>
{
    /// <summary>
    /// The unique identifier of the user to retrieve.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// The email address of the user to retrieve.
    /// </summary>
    public string? Email { get; set; }
}
