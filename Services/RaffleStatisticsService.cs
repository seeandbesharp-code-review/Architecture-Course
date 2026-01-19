using ChineseRaffleApi.Data;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Services.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Services
{
    public class RaffleStatisticsService : IRaffleStatisticsService
    {
      
            private readonly MyContext _context;

            public RaffleStatisticsService(MyContext context)
            {
                _context = context;
            }

            public async Task<ChineseRaffleSummaryDto> GetSummaryAsync()
            {
                var topGift = await _context.Gifts
                    .OrderByDescending(g => g.TicketList.Count())
                    .Select(g => g.Title)
                    .FirstOrDefaultAsync();

                return new ChineseRaffleSummaryDto
                {
                    TotalRevenue = await _context.Tickets.SumAsync(t => t.Gift.TicketPrice),
                    TotalTicketsSold = await _context.Tickets.CountAsync(),
                    TotalDonors = await _context.Donors.CountAsync(),
                    TotalGifts = await _context.Gifts.CountAsync(),
                    TopSellingGiftName = topGift ?? "No sales yet"
                };
            }
        
    }
}
