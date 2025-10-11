using HisuianArchives.Application.DTOs.Auth;

namespace HisuianArchives.Application.Services;

/// <summary>
/// Provides authentication services for user login.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with the provided login credentials.
    /// </summary>
    /// <param name="loginDto">The login request data transfer object.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing the authentication token and user profile.</returns>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
}