using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Mapping
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<AddBasketDto, Basket>();
            CreateMap<Basket, GetBasketDto>();
            CreateMap<Basket, UpdateBasketDto>();
        }
    }
}
