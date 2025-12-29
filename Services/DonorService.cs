using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace ChineseRaffleApi.Services
{
    public class DonorService:IDonorService
    {
        private readonly IDonorRepo  _donorRepo;
        private readonly IMapper _mapper;
        public DonorService(IDonorRepo donorRepo , IMapper mapper)
        { 
            _donorRepo = donorRepo;
            _mapper = mapper;
        }

        public async Task<int> AddDonorAsync(AddDonorDto donor)
        {
            Donor NewDonor = new Donor()
            {
                Name = donor.Name,
                PhoneNumber = donor.PhoneNumber,
                Email = donor.Email,
            };
            await _donorRepo.AddDonorAsync(NewDonor);
            return NewDonor.Id;
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            return await _donorRepo.DeleteDonorAsync(id);
        }

        public async Task<bool> DonorExistsAsync(string name)
        {
            return await _donorRepo.DonorExistsAsync(name);
        }

        public async Task<IEnumerable<GetDonorDto>> GetAllDonorsAsync()
        {
            var donors =  await _donorRepo.GetAllDonorsAsync();
            return _mapper.Map<List<GetDonorDto>>(donors);
        }

        public async Task<GetDonorDto?> GetDonorByIdAsync(int id)
        {
            var donor = await _donorRepo.GetDonorByIdAsync(id);
            return _mapper.Map<GetDonorDto>(donor);
        }
        public async Task<IEnumerable<GetDonorDto>> GetDonorByNameAsync(string name)
        {
            var donors = await _donorRepo.GetDonorByNameAsync(name);
            return _mapper.Map<List<GetDonorDto>>(donors);

        }
        public async Task<IEnumerable<GetDonorDto>> GetDonorByEmailAsync(string email)
        {
            var donors = await _donorRepo.GetDonorByEmailAsync(email);
            return _mapper.Map<List<GetDonorDto>>(donors);

        }
        public async Task<GetDonorDto?> GetDonorByGiftAsync(int giftId)
        {
            var donors = await _donorRepo.GetDonorByGiftAsync(giftId);
            return _mapper.Map<GetDonorDto>(donors);
        }

        public async Task UpdateDonorAsync(int id, UpdateDonorDto donor)
        {
            var existingDonor = await _donorRepo.GetDonorByIdAsync(id);
            if (existingDonor != null)
            {
                if (donor.Name != null) existingDonor.Name = donor.Name;
                if (donor.Email != null) existingDonor.Email = donor.Email;
                if (donor.PhoneNumber != null) existingDonor.PhoneNumber = donor.PhoneNumber;
                await _donorRepo.UpdateDonorAsync(id, existingDonor);
            }
        }

        //Task<IEnumerable<Donor>> IDonorService.GetDonorByGiftAsync(int giftId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
