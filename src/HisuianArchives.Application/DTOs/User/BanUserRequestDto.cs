namespace HisuianArchives.Application.DTOs.User;

/// <summary>
/// Data transfer object for banning a user request.
/// </summary>
public class BanUserRequestDto
{
    /// <summary>
    /// The reason for banning the user.
    /// </summary>
    public string Reason { get; set; } = null!;

    /// <summary>
    /// The date and time when the ban expires. If null, the ban is permanent.
    /// </summary>
    public DateTimeOffset? BannedUntil { get; set; }
}
