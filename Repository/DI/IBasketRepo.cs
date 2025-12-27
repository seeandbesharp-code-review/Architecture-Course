using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Repository.DI
{
    public interface IBasketRepo
    {
        Task<Basket> GetBasketByIdAsync(int id);
        Task<IEnumerable<Basket>> GetAllBasketsAsync();
        Task AddBasketAsync(Basket basket);
        Task UpdateBasketAsync(Basket basket);
        Task DeleteBasketAsync(int id);
        Task<bool> BasketExistsAsync(int basketId);
        Task<IEnumerable<Basket>> GetBasketsByUserIdAsync(int userId);
        Task<IEnumerable<Basket>> GetBasketsByGiftIdAsync(int giftId);


    }
}
