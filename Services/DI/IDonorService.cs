using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface IDonorService
    {
        Task<Donor?> GetDonorByIdAsync(int id);
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
        Task<int> AddDonorAsync(AddDonorDto donor);
        Task UpdateDonorAsync(int id,UpdateDonorDto donor);
        Task<bool> DeleteDonorAsync(int id);
        Task<bool> DonorExistsAsync(string name);
        Task<IEnumerable<Donor>> GetDonorByNameAsync(string name);
        Task<IEnumerable<Donor>> GetDonorByEmailAsync(string email);
        Task<Donor?> GetDonorByGiftAsync(int giftId);
    }
}
