using ChineseRaffleApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Repository.DI
{
    public interface IGiftRepo
    {
        Task<Gift?> GetGiftByIdAsync(int id);
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task AddGiftAsync(Gift gift);
        Task<bool> UpdateGiftAsync(int id,Gift gift);
        Task<bool> DeleteGiftAsync(int id);
        Task<bool> GiftExistsAsync(string title);
        Task<IEnumerable<Gift>> GetGiftByDonorNameAsync(string name);
        Task<Gift?> GetGiftByTitleAsync(string title);
        Task<IEnumerable<Gift>> GetGiftsWithTicketsAsync();
        Task<IEnumerable<Gift>> GetGiftsWithMaxPriceAsync();
        Task<IEnumerable<Gift>> GetGiftsWithMaxTicketsAsync();
        Task<IEnumerable<Gift>> GetSortedGiftsByPriceAsync();
        Task<IEnumerable<Gift>> GetSortedGiftsByCategoryAsync();
    }
}
