namespace HisuianArchives.Domain.Repositories;

/// <summary>
/// Defines methods for accessing and managing <see cref="Role"/> entities in the repository.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Asynchronously retrieves a <see cref="Role"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the role to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Role"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Role?> GetByNameAsync(string name);
}
