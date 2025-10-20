using AutoMapper;
using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
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

    public async Task<User> UpdateProfileAsync(Guid userId, string newName, string? newBio)
    {
        var user = await _userRepository.GetUserByIdAsync(userId) ??
            throw new BusinessException("User not found.");

        user.UpdateProfile(newName, newBio); 

        await _userRepository.UpdateAsync(user);

        return user!;
    }
}
