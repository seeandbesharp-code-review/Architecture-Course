using ChineseRaffleApi.Data;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class BasketRepo : IBasketRepo
    {

        MyContext _context;

        public BasketRepo(MyContext context)
        {
            _context = context;
        }
        public async Task<Basket?> GetBasketByIdAsync(int id)
        {
            return await _context.Baskets
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Basket>> GetAllBasketsAsync()
        {
            return await _context.Baskets
                .Include(b => b.Gift)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<int?> AddBasketAsync(Basket basket)
        {
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();
            return basket.Id;
        }

        public async Task UpdateBasketAsync(int id, Basket basket)
        {
            var existingBasket = await _context.Baskets
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBasket == null)
                throw new KeyNotFoundException($"Basket with id {id} not found");

            if (basket.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            existingBasket.Quantity = basket.Quantity;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBasketAsync(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);
            if (basket != null)
            {
                _context.Baskets.Remove(basket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> BasketExistsAsync(int basketId)
        {
            return await _context.Baskets.AnyAsync(b => b.Id == basketId);
        }

        public async Task<IEnumerable<Basket>> GetBasketsByUserIdAsync(int userId)
        {
            return await _context.Baskets
                .Include(b => b.Gift)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Basket>> GetBasketsByGiftIdAsync(int giftId)
        {
            return await _context.Baskets
                .Include(b => b.User)
                .Where(b => b.GiftId == giftId)
                .ToListAsync();
        }
        public async Task<Basket?> GetByUserAndGiftAsync(int userId, int giftId)
        {
            return await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.GiftId == giftId);
        }
    }
}
