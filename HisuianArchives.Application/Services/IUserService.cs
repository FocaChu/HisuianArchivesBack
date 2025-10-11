using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Services;

/// <summary>
/// Provides user management services including user creation and validation.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new user with the specified details and initial role.
    /// </summary>
    /// <param name="name">The user's name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="plainPassword">The user's password in plain text.</param>
    /// <param name="bio">The user's biography (optional).</param>
    /// <param name="initialRole">The initial role to assign to the user.</param>
    /// <returns>The created user entity with roles loaded.</returns>
    Task<User> CreateUserAsync(string name, string email, string plainPassword, string? bio, Role initialRole);
}
