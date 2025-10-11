using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Services;

/// <summary>
/// Service for generating authentication tokens for users.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A JWT token as a string.</returns>
    string GenerateJwtToken(User user);
}
