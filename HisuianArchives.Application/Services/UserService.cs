using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;

namespace HisuianArchives.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IPasswordService passwordService,  IMapper mapper, IImageRepository imageRepository)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _imageRepository = imageRepository;
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

    public async Task<User> UpdateUserProfileImageAsync(Guid userId, Guid imageId)
    {
        var image = await _imageRepository.GetByIdAsync(imageId);
        if (image == null)
        {
            throw new BusinessException("Image not found.");
        }

        if (image.OwnerId != userId)
        {
            throw new BusinessException("You do not have permission to use this image.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new BusinessException("User not found.");
        }

        user.SetProfileImage(imageId);

        await _userRepository.UpdateAsync(user);

        return user;
    }
}
