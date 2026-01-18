using ChineseRaffleApi.Data;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class GiftRepo : IGiftRepo
    {

        MyContext _context;

        public GiftRepo(MyContext context)
        {
            _context = context;
        }
        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts.Include(g => g.Donor).FirstOrDefaultAsync(g => g.Id == id);

        }

        public async Task<IEnumerable<Gift>> GetAllGiftsAsync()
        {
            return await _context.Gifts.Include(g => g.Donor).ToListAsync();
        }

        public async Task AddGiftAsync(Gift gift)
        {
            await _context.Gifts.AddAsync(gift);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateGiftAsync(int id, Gift gift)
        {
            try
            {
                var existingGift = await _context.Gifts.FindAsync(id);
                if (existingGift == null)
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(gift.Title))
                    existingGift.Title = gift.Title;

                existingGift.CategoryId = gift.CategoryId;

                if (gift.DonorId != 0)
                    existingGift.DonorId = gift.DonorId;

                if (gift.TicketPrice != 0)
                    existingGift.TicketPrice = gift.TicketPrice;

                existingGift.Image = gift.Image;

                existingGift.WinnerId = gift.WinnerId;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteGiftAsync(int id)
        {
            var gift = await GetGiftByIdAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> GiftExistsAsync(string title)
        {
            return await _context.Gifts.AnyAsync(g => g.Title == title);
        }
        public async Task<Gift?> GetGiftByTitleAsync(string title)
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .FirstOrDefaultAsync(gift => gift.Title.Contains(title));
        }
        public async Task<IEnumerable<Gift>> GetGiftByDonorNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Gift>();

            var trimmed = name.Trim();

            return await _context.Gifts
                .Include(g => g.Donor)
                .Where(g => g.Donor != null && EF.Functions.Like(g.Donor.Name, $"%{trimmed}%"))
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetGiftsWithTicketsAsync()
        {
            return await _context.Gifts
                .Include(g => g.TicketList)
                  .ThenInclude(t => t.User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetGiftsWithMaxPriceAsync()
        {
            var maxPrice = await _context.Gifts.MaxAsync(g => g.TicketPrice);

            return await _context.Gifts
                .Include(g => g.Donor)
                .Where(g => g.TicketPrice == maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gift>> GetGiftsWithMaxTicketsAsync()
        {
            var maxTickets = await _context.Gifts
                .MaxAsync(g => g.TicketList.Count());

            return await _context.Gifts
                .Include(g => g.Donor)
                .Where(g => g.TicketList.Count() == maxTickets)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetGiftsWithBuyersAsync()
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .Include(g => g.TicketList)
                  .ThenInclude(t => t.User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetSortedGiftsByPriceAsync()
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .OrderBy(g => g.TicketPrice)
                .ToListAsync();
        }
        public async Task<IEnumerable<Gift>> GetSortedGiftsByCategoryAsync()
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .OrderBy(g => g.CategoryId)
                .ToListAsync();
        }
    }
}

