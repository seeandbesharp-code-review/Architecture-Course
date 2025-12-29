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

        public RaffleController(IRaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        [HttpGet("download-raffle-zip")]
        public async Task<IActionResult> DownloadRaffleZip()
        {
            var zipBytes = await _raffleService.DrawRaffleFileAsync();
            return File(zipBytes, "application/zip", "raffle-results.zip");
        }
    }
}
