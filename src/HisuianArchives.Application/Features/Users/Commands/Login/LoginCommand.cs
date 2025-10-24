using MediatR;
using HisuianArchives.Application.DTOs.Auth;

namespace HisuianArchives.Application.Features.Users.Commands.Login;

/// <summary>
/// Command for authenticating a user and returning authentication information.
/// </summary>
public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
