using HisuianArchives.Application.DTOs.Auth;

namespace HisuianArchives.Application.Orchestrators;

/// <summary>
/// Orchestrates the user onboarding process including user creation and authentication.
/// </summary>
public interface IUserOnboardingOrchestrator
{
    /// <summary>
    /// Registers a new user and returns authentication information.
    /// </summary>
    /// <param name="registerDto">The registration request data transfer object.</param>
    /// <returns>The authentication response containing the JWT token and user profile.</returns>
    Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto registerDto);
}
