using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Domain.Repositories;

/// <summary>
/// Defines methods for accessing and managing user data in the repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Retrieves a user by their email address, including their associated roles.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user entity with roles if found; otherwise, null.</returns>
    Task<User?> GetUserByEmailWithRolesAsync(string email); 

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddUserAsync(User user);
}