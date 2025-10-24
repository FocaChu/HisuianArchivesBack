namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for user login requests.
/// </summary>
public class LoginRequestDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
