using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepo _basketRepo;
        public BasketService(IBasketRepo basketRepo)
        {
            _basketRepo = basketRepo;
        }
        public async Task<Basket> GetBasketByIdAsync(int id)
        {
            return await _basketRepo.GetBasketByIdAsync(id);
        }

        public async Task<IEnumerable<Basket>> GetAllBasketsAsync()
        {
            return await _basketRepo.GetAllBasketsAsync();
        }

        public async Task AddBasketAsync(Basket basket)
        {
            await _basketRepo.AddBasketAsync(basket);
        }

        public async Task UpdateBasketAsync(Basket basket)
        {
            await _basketRepo.UpdateBasketAsync(basket);
        }

        public async Task DeleteBasketAsync(int id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }

        public async Task<bool> BasketExistsAsync(int id)
        {
            return await _basketRepo.BasketExistsAsync(id);
        }

        public async Task<IEnumerable<Basket>> GetBasketsByUserIdAsync(int userId)
        {
            return await _basketRepo.GetBasketsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Basket>> GetBasketsByGiftIdAsync(int giftId)
        {
            return await _basketRepo.GetBasketsByGiftIdAsync(giftId);
        }


    }
}
