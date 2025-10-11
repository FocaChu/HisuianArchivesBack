using HisuianArchives.Application.Exceptions;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<User> CreateUserAsync(string name, string email, string plainPassword, string? bio, Role initialRole)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new BusinessException("A user with this email already exists.");
        }

        var passwordHash = _passwordService.HashPassword(plainPassword);

        var newUser = new User(name, email, passwordHash, bio);

        if (initialRole != null)
        {
            newUser.AddRole(initialRole);
        }

        await _userRepository.AddUserAsync(newUser);

        var userWithRoles = await _userRepository.GetUserByEmailWithRolesAsync(email);
        return userWithRoles!;
    }
}
