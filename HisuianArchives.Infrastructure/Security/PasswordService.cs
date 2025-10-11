using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HisuianArchives.Infrastructure.Security;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string providedPassword, string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
