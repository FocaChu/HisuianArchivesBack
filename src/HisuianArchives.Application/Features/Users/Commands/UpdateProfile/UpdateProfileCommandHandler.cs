using MediatR;
using HisuianArchives.Application.DTOs.User;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfile;

/// <summary>
/// Handler for the UpdateProfileCommand that updates a user's profile information.
/// </summary>
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UpdateProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserSummaryResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating profile for user {UserId}", request.UserId);

        // Get the user
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("Profile update failed: user {UserId} not found", request.UserId);
            throw new BusinessException("User not found.");
        }

        // Update the profile
        user.UpdateProfile(request.Name, request.Bio);

        // Persist the changes
        await _userRepository.UpdateAsync(user);

        // Map to response DTO
        var responseDto = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("Profile updated successfully for user {UserId}", request.UserId);

        return responseDto;
    }
}
