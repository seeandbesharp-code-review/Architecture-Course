using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Repository.DI
{
    public interface IDonorRepo
    {
        Task<Donor?> GetDonorByIdAsync(int id);
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
        Task<int> AddDonorAsync(Donor donor);
        Task<bool> UpdateDonorAsync(int id, Donor donor);
        Task<bool> DeleteDonorAsync(int id);
        Task<bool> DonorExistsAsync(string name);
        Task<IEnumerable<Donor>> GetDonorByNameAsync(string name);
        Task<IEnumerable<Donor>> GetDonorByEmailAsync(string email);
        Task<Donor?> GetDonorByGiftAsync(int giftId);
    }
}
