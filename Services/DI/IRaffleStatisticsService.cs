using ChineseRaffleApi.Dto;

namespace ChineseRaffleApi.Services.DI
{
    public interface IRaffleStatisticsService
    {
       
            Task<ChineseRaffleSummaryDto> GetSummaryAsync();
      
    }
}
