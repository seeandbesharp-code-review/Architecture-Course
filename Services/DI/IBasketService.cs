using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface IBasketService
    {
        Task<GetBasketDto?> GetBasketByIdAsync(int id);
        //Task<IEnumerable<GetBasketDto>> GetAllBasketsAsync();
        Task<int?> AddBasketAsync(AddBasketDto basket);
        Task UpdateBasketAsync(int id, UpdateBasketDto basket);
        Task DeleteBasketAsync(int id);
        Task<bool> BasketExistsAsync(int basketId);
        Task<IEnumerable<GetBasketDto>> GetBasketsByUserIdAsync(int userId);
        Task<IEnumerable<GetBasketDto>> GetBasketsByGiftIdAsync(int giftId);
        Task BuyTicketsFromBasket(int userId);
    }
}
