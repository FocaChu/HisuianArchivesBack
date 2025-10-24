using MediatR;
using HisuianArchives.Application.DTOs.Auth;

namespace HisuianArchives.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Command for creating a new user account.
/// </summary>
public class CreateUserCommand : IRequest<AuthResponseDto>
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
