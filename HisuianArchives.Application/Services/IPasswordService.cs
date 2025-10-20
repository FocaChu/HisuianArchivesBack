namespace HisuianArchives.Application.Services;

public interface IPasswordService
{
    /// <summary>
    /// Generates the hash of a plain text password.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verifies if a plain text password matches an existing hash.
    /// </summary>
    bool VerifyPassword(string providedPassword, string passwordHash);
}
