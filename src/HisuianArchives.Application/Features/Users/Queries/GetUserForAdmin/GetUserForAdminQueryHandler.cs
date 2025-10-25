using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.Queries.GetUserForAdmin;

/// <summary>
/// Handler for the GetUserForAdminQuery that retrieves a user by ID or email for admin purposes.
/// </summary>
public class GetUserForAdminQueryHandler : IRequestHandler<GetUserForAdminQuery, AdminUserSummaryResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserForAdminQueryHandler> _logger;

    public GetUserForAdminQueryHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserForAdminQueryHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AdminUserSummaryResponseDto> Handle(GetUserForAdminQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving user for admin - UserId: {UserId}, Email: {Email}", 
            request.UserId, request.Email);

        User? user = null;

        // Search by UserId if provided
        if (request.UserId.HasValue)
        {
            user = await _userRepository.GetUserByIdAsync(request.UserId.Value);
            _logger.LogInformation("Searching user by ID: {UserId}", request.UserId.Value);
        }
        // Search by Email if provided
        else if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user = await _userRepository.GetUserByEmailWithRolesAsync(request.Email);
            _logger.LogInformation("Searching user by email: {Email}", request.Email);
        }

        if (user == null)
        {
            _logger.LogWarning("User not found - UserId: {UserId}, Email: {Email}", 
                request.UserId, request.Email);
            throw new BusinessException("User not found.");
        }

        // Map to admin response DTO
        var responseDto = _mapper.Map<AdminUserSummaryResponseDto>(user);

        _logger.LogInformation("User retrieved successfully for admin - UserId: {UserId}", user.Id);

        return responseDto;
    }
}
