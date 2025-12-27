using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepo _giftRepo;

        public GiftService(IGiftRepo giftRepo)
        {
            _giftRepo = giftRepo;
        }

        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _giftRepo.GetGiftByIdAsync(id);
        }

        public async Task<IEnumerable<Gift>> GetAllGiftsAsync()
        {
            return await _giftRepo.GetAllGiftsAsync();
        }

        public async Task<int> AddGiftAsync(AddGiftDto gift)
        {
            var trimmedTitle = gift.Title?.Trim() ?? string.Empty;
            if (await _giftRepo.GiftExistsAsync(trimmedTitle))
            {
                throw new ArgumentException($"Gift title '{trimmedTitle}' is already existing.");
            }
            var newGift = new Gift
            {
                Title = gift.Title,
                CategoryId = gift.CategoryId,
                DonorId = gift.DonorId,
                TicketPrice = gift.TicketPrice,
                Image = gift.Image,
            };
            await _giftRepo.AddGiftAsync(newGift);
            return newGift.Id; 
        }

        public async Task<bool> UpdateGiftAsync(int id, UpdateGiftDto gift)
        {

            var existing = await _giftRepo.GetGiftByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Gift with id {id} was not found.");

            if (!string.IsNullOrWhiteSpace(gift.Title))
            {
                var trimmedTitle = gift.Title.Trim();
                // If the trimmed title differs from the existing title (case-insensitive), check uniqueness
                if (!string.Equals(trimmedTitle, existing.Title, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (await _giftRepo.GiftExistsAsync(trimmedTitle))
                        throw new ArgumentException($"Gift title '{trimmedTitle}' is already existing.");
                }

                existing.Title = trimmedTitle;
            }
            if (gift.CategoryId.HasValue)
                existing.CategoryId = gift.CategoryId;

            if (gift.DonorId.HasValue)
                existing.DonorId = gift.DonorId.Value;

            if (gift.TicketPrice.HasValue)
                existing.TicketPrice = gift.TicketPrice.Value;

            if (gift.Image != null)
                existing.Image = gift.Image;

            if (gift.WinnerId.HasValue)
                existing.WinnerId = gift.WinnerId;

            var updated = await _giftRepo.UpdateGiftAsync(id, existing);
            if (!updated)
                throw new InvalidOperationException($"Failed to update gift with id {id}.");
            return true;

        }

        public async Task<bool> DeleteGiftAsync(int id)
        {
            return await _giftRepo.DeleteGiftAsync(id);
        }

        public async Task<bool> GiftExistsAsync(string title)
        {
            return await _giftRepo.GiftExistsAsync(title);
        }

        public Task<IEnumerable<Gift>> GetGiftByDonorNameAsync(string name)
        {
            return _giftRepo.GetGiftByDonorNameAsync(name);
        }

        public Task<IEnumerable<Gift>> GetGiftByTitleAsync(string title)
        {
            return _giftRepo.GetGiftByTitleAsync(title);
        }
    }
}
