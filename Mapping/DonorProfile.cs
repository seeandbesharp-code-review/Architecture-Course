using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Mapping
{
    public class DonorProfile : Profile
    {
        public DonorProfile() 
        {
            CreateMap<Donor, GetDonorDto>();
            CreateMap<AddDonorDto,Donor>();
            CreateMap<Donor, UpdateDonorDto>();     
        }
    }
}
