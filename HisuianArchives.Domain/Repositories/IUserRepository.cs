namespace HisuianArchives.Domain.Repositories;

/// <summary>
/// Defines methods for accessing, retrieving, adding, and updating user data in the repository,
/// including operations to fetch users by ID or email, retrieve users with their associated roles,
/// and manage user entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetUserByIdAsync(Guid id);

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


    /// <summary>
    /// Updates the details of an existing user in the repository.
    /// </summary>
    /// <param name="user">The user entity with updated information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(User user);
}