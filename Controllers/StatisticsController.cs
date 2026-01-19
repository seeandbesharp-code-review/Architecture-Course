using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IRaffleStatisticsService _statsService;

        public StatisticsController(IRaffleStatisticsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ChineseRaffleSummaryDto>> GetSummary()
        {
            var summary = await _statsService.GetSummaryAsync();
            return Ok(summary);
        }
    }
}
