using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface IGiftService
    {
        Task<Gift?> GetGiftByIdAsync(int id);
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task<int> AddGiftAsync(AddGiftDto gift);
        Task<bool> UpdateGiftAsync(int id, UpdateGiftDto gift);
        Task<bool> DeleteGiftAsync(int id);
        Task<bool> GiftExistsAsync(string title);
        Task<IEnumerable<Gift>> GetGiftByDonorNameAsync(string name);
        Task<IEnumerable<Gift>> GetGiftByTitleAsync(string title);

    }
}
