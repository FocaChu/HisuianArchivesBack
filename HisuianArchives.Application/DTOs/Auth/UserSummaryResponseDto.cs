namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Represents a summary of user information for response purposes.
/// </summary>
public class UserSummaryResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the biography of the user.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the user's profile image.
    /// </summary>
    public Guid? ProfileImageId { get; set; }
}
