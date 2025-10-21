using AutoMapper;
using HisuianArchives.Application.DTOs.Auth;
using HisuianArchives.Application.DTOs.Image;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Features.Images.Commands.UploadImage;
using HisuianArchives.Application.Features.Users.Commands.CreateUser;
using HisuianArchives.Application.Features.Users.Commands.Login;
using HisuianArchives.Application.Features.Users.Commands.UpdateProfile;
using HisuianArchives.Application.Features.Users.Commands.UpdateProfileImage;

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
        
        // Map LoginRequestDto to LoginCommand
        CreateMap<LoginRequestDto, LoginCommand>();
        
        // Map UserUpdateProfileRequestDto to UpdateProfileCommand
        CreateMap<UserUpdateProfileRequestDto, UpdateProfileCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Will be set from the authenticated user
            
        // Map UpdateProfileImageRequestDto to UpdateProfileImageCommand
        CreateMap<UpdateProfileImageRequestDto, UpdateProfileImageCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Will be set from the authenticated user
            
        // Map ImageUploadRequestDto to UploadImageCommand
        CreateMap<ImageUploadRequestDto, UploadImageCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Will be set from the authenticated user
    }
}
