using MediatR;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Handler for the GetUserByIdQuery that retrieves a user by their ID.
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserSummaryResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving user with ID {UserId}", request.UserId);

        // Get the user
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("User retrieval failed: user {UserId} not found", request.UserId);
            throw new BusinessException("User not found.");
        }

        // Map to response DTO
        var responseDto = _mapper.Map<UserSummaryResponseDto>(user);

        _logger.LogInformation("User {UserId} retrieved successfully", request.UserId);

        return responseDto;
    }
}


