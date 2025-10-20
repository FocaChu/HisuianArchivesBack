namespace HisuianArchives.Application.Interfaces;

/// <summary>
/// Defines a service to obtain information about the user authenticated in the current session.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Gets the user ID of the current session.
    /// </summary>
    /// <returns>The user's ID as a string, or null if the user is not authenticated.</returns>
    string? GetUserId();
}