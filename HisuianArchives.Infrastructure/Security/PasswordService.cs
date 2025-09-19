using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HisuianArchives.Infrastructure.Security;

public class PasswordService : IPasswordService
{
    // Usamos a ferramenta poderosa da Microsoft por baixo dos panos
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(string password)
    {
        // O null! é para satisfazer o método que espera uma instância de User,
        // mas que na verdade não a utiliza para a operação de hash.
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string providedPassword, string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
