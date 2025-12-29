using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<AddUserDto, User>();
            CreateMap<User, UpdateUserDto>();
        }
    }
}
