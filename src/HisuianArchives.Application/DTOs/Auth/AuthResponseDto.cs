using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Represents the authentication response containing the JWT token and the authenticated user's summary profile.
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = null!;

    public UserSummaryResponseDto UserProfile { get; set; } = null!;
}
