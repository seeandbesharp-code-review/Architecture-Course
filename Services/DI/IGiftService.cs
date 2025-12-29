using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface IGiftService
    {
        Task<GetGiftDto?> GetGiftByIdAsync(int id);
        Task<IEnumerable<GetGiftDto>> GetAllGiftsAsync();
        Task<int> AddGiftAsync(AddGiftDto gift);
        Task<bool> UpdateGiftAsync(int id, UpdateGiftDto gift);
        Task<bool> DeleteGiftAsync(int id);
        Task<bool> GiftExistsAsync(string title);
        Task<IEnumerable<GetGiftDto>> GetGiftByDonorNameAsync(string name);
        Task<GetGiftDto?> GetGiftByTitleAsync(string title);
        Task<IEnumerable<GetGiftWithTicketsDto>> GetGiftsWithTicketsAsync();
        Task<IEnumerable<GetGiftDto>> GetGiftsWithMaxPriceAsync();
        Task<IEnumerable<GetGiftDto>> GetGiftsWithMaxTicketsAsync();
        Task<IEnumerable<GetGiftWithBuyersDto>> GetGiftsWithBuyersAsync();
    }
}
