using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface IDonorService
    {
        Task<GetDonorDto?> GetDonorByIdAsync(int id);
        Task<IEnumerable<GetDonorDto>> GetAllDonorsAsync();
        Task<int> AddDonorAsync(AddDonorDto donor);
        Task UpdateDonorAsync(int id,UpdateDonorDto donor);
        Task<bool> DeleteDonorAsync(int id);
        Task<bool> DonorExistsAsync(string name);
        Task<IEnumerable<GetDonorDto>> GetDonorByNameAsync(string name);
        Task<IEnumerable<GetDonorDto>> GetDonorByEmailAsync(string email);
        Task<GetDonorDto?> GetDonorByGiftAsync(int giftId);
    }
}
