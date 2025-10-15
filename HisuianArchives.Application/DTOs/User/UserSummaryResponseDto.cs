namespace HisuianArchives.Application.DTOs.User;

/// <summary>
/// Represents a summary of user information for response purposes.
/// </summary>
public class UserSummaryResponseDto
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
