using AutoMapper;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.Features.Users.Commands.CreateUser;

namespace HisuianArchives.Application.Mappers;

/// <summary>
/// AutoMapper profile for mapping between DTOs and Commands.
/// </summary>
public class CommandProfile : Profile
{
    public CommandProfile()
    {
        // Map RegisterRequestDto to CreateUserCommand
        CreateMap<RegisterRequestDto, CreateUserCommand>();
    }
}
