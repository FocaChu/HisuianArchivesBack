namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Represents the authentication response containing the JWT token and the authenticated user's summary profile.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// The JWT token generated for user authentication.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// The summary profile of the authenticated user.
    /// </summary>
    public UserSummaryResponseDto UserProfile { get; set; } = null!;
}
