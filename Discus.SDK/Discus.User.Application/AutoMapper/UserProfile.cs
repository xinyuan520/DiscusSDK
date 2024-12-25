using Discus.User.Application.Contracts.Dtos;
using Discus.User.Repository.Entities;

namespace Discus.User.Application.AutoMapper
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfo>();

            CreateMap<UserInfo, UserInfoRequestDto>();
            CreateMap<UserInfoRequestDto, UserInfo>();
        }
    }
}
