using MediatR;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfileImage;

/// <summary>
/// Handler for the UpdateProfileImageCommand that updates a user's profile image.
/// </summary>
public class UpdateProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommand, UserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProfileImageCommandHandler> _logger;

    public UpdateProfileImageCommandHandler(
        IUserRepository userRepository,
        IImageRepository imageRepository,
        IMapper mapper,
        ILogger<UpdateProfileImageCommandHandler> logger)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserSummaryResponseDto> Handle(UpdateProfileImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating profile image for user {UserId} to image {ImageId}", request.UserId, request.ImageId);

        // Verify the image exists
        var image = await _imageRepository.GetByIdAsync(request.ImageId);
        if (image == null)
        {
            _logger.LogWarning("Profile image update failed: image {ImageId} not found", request.ImageId);
            throw new BusinessException("Image not found.");
        }

        // Verify the user owns the image
        if (image.OwnerId != request.UserId)
        {
            _logger.LogWarning("Profile image update failed: user {UserId} does not own image {ImageId}", request.UserId, request.ImageId);
            throw new BusinessException("You do not have permission to use this image.");
        }

        // Get the user
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("Profile image update failed: user {UserId} not found", request.UserId);
            throw new BusinessException("User not found.");
        }

        // Update the profile image
        user.SetProfileImage(request.ImageId);

        // Persist the changes
        await _userRepository.UpdateAsync(user);

        // Map to response DTO
        var responseDto = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("Profile image updated successfully for user {UserId} to image {ImageId}", request.UserId, request.ImageId);

        return responseDto;
    }
}
