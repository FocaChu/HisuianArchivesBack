namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for user registration requests.
/// </summary>
public class RegisterRequestDto
{
    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
