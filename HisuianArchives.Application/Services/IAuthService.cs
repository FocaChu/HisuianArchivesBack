using HisuianArchives.Application.DTOs.Auth;

namespace HisuianArchives.Application.Services;

/// <summary>
/// Provides authentication services such as login and registration.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with the provided login credentials.
    /// </summary>
    /// <param name="loginDto">The login request data transfer object.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing the authentication token and user profile.</returns>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="registerDto">The registration request data transfer object.</param>
    /// <returns>A string containing the result of the registration process.</returns>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto);
}