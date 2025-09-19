namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for user registration requests.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the bio of the user.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = null!;
}
