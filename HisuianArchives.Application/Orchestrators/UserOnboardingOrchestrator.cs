using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Orchestrators;

public class UserOnboardingOrchestrator : IUserOnboardingOrchestrator
{
    private readonly IUserService _userService;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public UserOnboardingOrchestrator(
        IUserService userService,
        IRoleRepository roleRepository,
        ITokenService tokenService,
        IMapper mapper,
        IEmailService emailService)
    {
        _userService = userService;
        _roleRepository = roleRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto registerDto)
    {
        var defaultRole = await _roleRepository.GetByNameAsync("Customer");
        if (defaultRole == null)
        {
            throw new InvalidOperationException("Default Customer role not found in the system.");
        }

        var newUser = await _userService.CreateUserAsync(
            registerDto.Name,
            registerDto.Email,
            registerDto.Password,
            registerDto.Bio,
            defaultRole);

        try
        {
            await _emailService.SendWelcomeEmailAsync(newUser.Email, newUser.Name);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to send welcome email to {newUser.Email}. Error: {ex.Message}");
        }

        var token = _tokenService.GenerateJwtToken(newUser);

        var userProfile = _mapper.Map<UserSummaryResponseDto>(newUser);

        return new AuthResponseDto
        {
            Token = token,
            UserProfile = userProfile
        };
    }
}
