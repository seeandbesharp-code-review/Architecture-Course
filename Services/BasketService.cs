using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepo basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        public async Task<GetBasketDto?> GetBasketByIdAsync(int id)
        {
            var basket = await _basketRepo.GetBasketByIdAsync(id);
            return _mapper.Map<GetBasketDto>(basket);
        }
        public async Task<IEnumerable<GetBasketDto>> GetBasketsByUserIdAsync(int userId)
        {
            var basket = await _basketRepo.GetBasketsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GetBasketDto>>(basket);
        }

        public async Task<IEnumerable<GetBasketDto>> GetBasketsByGiftIdAsync(int giftId)
        {
            var basket = await _basketRepo.GetBasketsByGiftIdAsync(giftId);
            return _mapper.Map<IEnumerable<GetBasketDto>>(basket);

        }
        //public async Task<IEnumerable<Basket>> GetAllBasketsAsync()
        //{
        //    return await _basketRepo.GetAllBasketsAsync();
        //}

        public async Task<int?> AddBasketAsync(AddBasketDto basketDto)
        {
            var basket = _mapper.Map<Basket>(basketDto);

            var existingBasket = await _basketRepo
                .GetByUserAndGiftAsync(basket.UserId, basket.GiftId);

            if (existingBasket != null)
            {
                existingBasket.Quantity += basket.Quantity;
                await _basketRepo.UpdateBasketAsync(existingBasket.Id, existingBasket);
                return existingBasket.Id;
            }
            return await _basketRepo.AddBasketAsync(basket);
        }

        public async Task UpdateBasketAsync(int id, UpdateBasketDto basket)
        {
            if (basket.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var existingBasket = await _basketRepo.GetBasketByIdAsync(id);

            if (existingBasket == null)
                throw new KeyNotFoundException($"Basket with id {id} not found");

            existingBasket.Quantity = basket.Quantity;

            await _basketRepo.UpdateBasketAsync(id, existingBasket);
        }

        public async Task DeleteBasketAsync(int id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }

        public async Task<bool> BasketExistsAsync(int id)
        {
            return await _basketRepo.BasketExistsAsync(id);
        }
    }
}
