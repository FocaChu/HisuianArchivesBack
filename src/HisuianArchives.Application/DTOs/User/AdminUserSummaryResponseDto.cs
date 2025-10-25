namespace HisuianArchives.Application.DTOs.User;

/// <summary>
/// Represents a summary of user information for admin purposes, including ban status and roles.
/// </summary>
public class AdminUserSummaryResponseDto
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    public string Email { get; set; } = null!;

    public Guid? ProfileImageId { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset LastModified { get; set; }

    /// <summary>
    /// Indicates if the user account is active (not banned).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The reason for the ban, if the user is banned.
    /// </summary>
    public string? BannedReason { get; set; }

    /// <summary>
    /// The date and time when the ban expires. If null, the ban is permanent.
    /// </summary>
    public DateTimeOffset? BannedUntil { get; set; }

    /// <summary>
    /// List of role names assigned to the user.
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
