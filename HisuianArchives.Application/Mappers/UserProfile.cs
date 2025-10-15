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
        }
    }
}
