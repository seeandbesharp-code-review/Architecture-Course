using AutoMapper;
using ChineseRaffleApi.Models;
using static ChineseRaffleApi.Dto.CategoryDto;

namespace ChineseRaffleApi.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoryDto>();
            CreateMap<Category, GetCategoryWithGiftsDto>();
        }
    }
}
