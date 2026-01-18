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
        .ForMember(dest => dest.QuantitySold,
            opt => opt.MapFrom(src => src.TicketList == null ? 0 : src.TicketList.Count()))
        .ForMember(dest => dest.Buyers, opt => opt.MapFrom((src, dest, destMember, context) =>
        {
            if (src.TicketList == null || !src.TicketList.Any())
                return new List<GetUserDto>();
            var uniqueUsers = src.TicketList
                .Where(t => t.User != null)
                .Select(t => t.User)
                .GroupBy(u => u.Id)
                .Select(group => group.First())
                .ToList();
            return context.Mapper.Map<IEnumerable<GetUserDto>>(uniqueUsers);
        }));
            CreateMap<Gift, GetGiftForDonorDto>();
        }
    }
}
