using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Interfaces;

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

    /// <summary>
    /// Updates the profile information of an existing user, including name and biography.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to update.</param>
    /// <param name="newName">The new name for the user.</param>
    /// <param name="newBio">The new biography for the user (optional).</param>
    /// <returns>The updated user entity.</returns>
    Task<User> UpdateProfileAsync(Guid userId, string newName, string? newBio);
}
