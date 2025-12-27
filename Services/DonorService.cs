using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class DonorService:IDonorService
    {
        private readonly IDonorRepo  _donorRepo;
        public DonorService(IDonorRepo donorRepo)
        { 
            _donorRepo = donorRepo;
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

        public async Task<IEnumerable<Donor>> GetAllDonorsAsync()
        {
            return await _donorRepo.GetAllDonorsAsync();
        }

        public async Task<Donor?> GetDonorByIdAsync(int id)
        {
            return await _donorRepo.GetDonorByIdAsync(id);
        }
        public async Task<IEnumerable<Donor>> GetDonorByNameAsync(string name)
        {
            return await _donorRepo.GetDonorByNameAsync(name);
        }
        public async Task<IEnumerable<Donor>> GetDonorByEmailAsync(string email)
        {
            return await _donorRepo.GetDonorByEmailAsync(email);
        }
        public async Task<Donor?> GetDonorByGiftAsync(int giftId)
        {
            return await _donorRepo.GetDonorByGiftAsync(giftId);
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
