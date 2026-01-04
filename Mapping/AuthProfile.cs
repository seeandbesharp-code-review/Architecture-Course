using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<GetUserDto, LoginRequestDto>();
            
        }

    }
}
