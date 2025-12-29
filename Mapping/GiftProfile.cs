using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using AutoMapper;


namespace ChineseRaffleApi.Mapping
{
    public class GiftProfile : Profile
    {
        public GiftProfile()
        {
            CreateMap<Gift, GetGiftDto>();
            CreateMap<AddGiftDto ,Gift>();  
            CreateMap<Gift, UpdateGiftDto>();
            CreateMap<Gift, GetGiftWithTicketsDto>()
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.TicketList))
            .ForMember(dest => dest.QuantitySold,
                    opt => opt.MapFrom(src => src.TicketList == null ? 0 : src.TicketList.Count()));
            CreateMap<Gift, GetGiftWithBuyersDto>()
            .ForMember(dest => dest.Buyers, opt => opt.MapFrom(src =>
              src.TicketList == null
              ? new List<User>()
               : src.TicketList
               .Select(t => t.User)));
        }
    }
}
