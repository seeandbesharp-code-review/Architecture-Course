using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly IRaffleService _raffleService;
        private readonly ILogger<RaffleController> _logger;

        public RaffleController(IRaffleService raffleService, ILogger<RaffleController> logger)
        {
            _raffleService = raffleService;
            _logger = logger;
        }

        [HttpGet("download-raffle-zip")]
        public async Task<IActionResult> DownloadRaffleZip()
        {
            try
            {
                var zipBytes = await _raffleService.DrawRaffleFileAsync();

                if (zipBytes == null || zipBytes.Length == 0)
                {
                    return NotFound("No raffle data available to download.");
                }

                return File(zipBytes, "application/zip", "raffle-results.zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating raffle zip file.");
                return StatusCode(500, "An error occurred while generating the raffle file.");
            }
        }

    }
}
