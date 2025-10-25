using AutoMapper;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {

            CreateMap<User, UserSummaryResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, AdminUserSummaryResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToList())); 
        }
    }
}
