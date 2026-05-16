using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ChineseRaffleApi.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepo _giftRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;


        public GiftService(IGiftRepo giftRepo, IMapper mapper, IDistributedCache cache, IConfiguration config)
        {
            _giftRepo = giftRepo;
            _mapper = mapper;
            _cache = cache;
            _config = config;
        }

        public async Task<GetGiftDto?> GetGiftByIdAsync(int id)
        {
            var cacheKey = $"gift:{id}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            
            if (!string.IsNullOrEmpty(cachedData))
            {
                try
                {
                    var cachedGift = JsonSerializer.Deserialize<GetGiftDto>(cachedData);
                    if (cachedGift != null)
                        return cachedGift;
                }
                catch
                {
                    // Invalid cache data, proceed to database fetch
                }
            }
            
            var gift = await _giftRepo.GetGiftByIdAsync(id);
            if (gift == null) return null;
            
            var dto = _mapper.Map<GetGiftDto>(gift);
            var ttl = int.Parse(_config["RedisSettings:DefaultTTL"] ?? "300");
            
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl)
            });
            
            return dto;
        }

        public async Task<IEnumerable<GetGiftDto>> GetAllGiftsAsync()
        {
            var gifts = await _giftRepo.GetAllGiftsAsync();
            return _mapper.Map<List<GetGiftDto>>(gifts);
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
            
            // Invalidate cache
            await _cache.RemoveAsync($"gift:{id}");
            
            return true;

        }

        public async Task<bool> DeleteGiftAsync(int id)
        {
            var result = await _giftRepo.DeleteGiftAsync(id);
            
            // Invalidate cache
            await _cache.RemoveAsync($"gift:{id}");
            
            return result;
        }

        public async Task<bool> GiftExistsAsync(string title)
        {
            return await _giftRepo.GiftExistsAsync(title);
        }

        public async Task<IEnumerable<GetGiftDto>> GetGiftByDonorNameAsync(string name)
        {
            var gifts = await _giftRepo.GetGiftByDonorNameAsync(name);
            return _mapper.Map<List<GetGiftDto>>(gifts);
        }

        public async Task<GetGiftDto?> GetGiftByTitleAsync(string title)
        {
            var gift = await _giftRepo.GetGiftByTitleAsync(title);
            return _mapper.Map<GetGiftDto>(gift);
        }
        public async Task<IEnumerable<GetGiftWithTicketsDto>> GetGiftsWithTicketsAsync()
        {
            var gifts = await _giftRepo.GetGiftsWithTicketsAsync();
            return _mapper.Map<IEnumerable<GetGiftWithTicketsDto>>(gifts);
        }
        public async Task<IEnumerable<GetGiftDto>> GetGiftsWithMaxPriceAsync()
        {
            var gifts = await _giftRepo.GetGiftsWithMaxPriceAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }
        public async Task<IEnumerable<GetGiftDto>> GetGiftsWithMaxTicketsAsync()
        {
            var gifts = await _giftRepo.GetGiftsWithMaxTicketsAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }
        public async Task<IEnumerable<GetGiftWithBuyersDto>> GetGiftsWithBuyersAsync()
        {

            var gifts = await _giftRepo.GetGiftsWithBuyersAsync();
            
            return _mapper.Map<IEnumerable<GetGiftWithBuyersDto>>(gifts);
        }
        public async Task<IEnumerable<GetGiftDto>> GetSortedGiftsByPriceAsync()
        {
            var gifts = await _giftRepo.GetSortedGiftsByPriceAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }
        public async Task<IEnumerable<GetGiftDto>> GetSortedGiftsByCategoryAsync()
        {
            var gifts = await _giftRepo.GetSortedGiftsByCategoryAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }
    }
}
