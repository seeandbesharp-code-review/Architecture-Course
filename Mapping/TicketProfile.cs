using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
             CreateMap<Ticket, GetTicketDto>()
            .ForMember(dest => dest.GiftTitle, opt => opt.MapFrom(src => src.Gift.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
             CreateMap< AddTicketDto, Ticket>();
        }
    }
}
