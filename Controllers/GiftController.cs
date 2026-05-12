using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Drawing;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly ILogger<GiftController> _logger;

        public GiftController(IGiftService giftService, ILogger<GiftController> logger)
        {
            _giftService = giftService;
            _logger = logger;
        }
        [EnableRateLimiting("sliding")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetGiftDto>> GetGift(int id)
        {
            try
            {
                var gift = await _giftService.GetGiftByIdAsync(id);
                if (gift == null)
                {
                    return NotFound();
                }
                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gift with id {GiftId}", id);
                return StatusCode(500, "An unexpected error occurred while fetching the gift.");
            }
        }

        [EnableRateLimiting("sliding")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetAllGifts()
        {
            try
            {
                var gifts = await _giftService.GetAllGiftsAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all gifts");
                return StatusCode(500, new { message = "An error occurred while retrieving gifts." });
            }
        }

        [EnableRateLimiting("sliding")]
        [HttpGet("sorted/price")]
        public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetGiftsSortedByPrice()
        {
            try
            {
                var gifts = await _giftService.GetSortedGiftsByPriceAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gifts sorted by price");
                return BadRequest(new { message = $"Error fetching gifts sorted by price: {ex.Message}" });
            }
        }

        [EnableRateLimiting("sliding")]
        [HttpGet("sorted/category")]
        public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetGiftsSortedByCategory()
        {
            try
            {
                var gifts = await _giftService.GetSortedGiftsByCategoryAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gifts sorted by category");
                return BadRequest(new { message = $"Error fetching gifts sorted by category: {ex.Message}" });
            }
        }



        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpPost] 
        public async Task<ActionResult<Gift>> AddGift([FromForm] AddGiftUploadDto giftDto)
        {
            try
            {
                if (giftDto.ImageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(giftDto.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gifts", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await giftDto.ImageFile.CopyToAsync(stream);
                    }
                    giftDto.Image = $"images/gifts/{fileName}";
                }

                var id = await _giftService.AddGiftAsync(giftDto);
                return CreatedAtAction(nameof(GetGift), new { id = id }, giftDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding gift");
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGift(int id, [FromForm] UpdateGiftUploadDto giftDto)
        {
            try
            {
                var existingGift = await _giftService.GetGiftByIdAsync(id);
                if (existingGift == null) return NotFound($"Gift with id {id} not found.");

                if (giftDto.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(existingGift.Image))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingGift.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(giftDto.ImageFile.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "gifts");

                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await giftDto.ImageFile.CopyToAsync(stream);
                    }

                    giftDto.Image = $"images/gifts/{fileName}";
                }
                else
                {
                    giftDto.Image = existingGift.Image;
                }

                await _giftService.UpdateGiftAsync(id, giftDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Validation error updating gift {GiftId}", id);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating gift {GiftId}", id);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                var isDeleted = await _giftService.DeleteGiftAsync(id);
                if (isDeleted)
                {
                    return Ok(new { message = "Gift deleted successfully", Id = id });
                }
                return BadRequest(new { message = "Failed to delete" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting gift with id {GiftId}", id);
                return StatusCode(500, "An unexpected error occurred while deleting the gift.");
            }
        }

        [HttpGet("exists/{title}")]
        public async Task<ActionResult<bool>> GiftExists(string title)
        {
            try
            {
                var exists = await _giftService.GiftExistsAsync(title);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if gift exists with title {Title}", title);
                return StatusCode(500, "An unexpected error occurred while checking gift existence.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("donor/{name}")]
        public async Task<ActionResult<IEnumerable<Gift>>> GetGiftsByDonorName(string name)
        {
            try
            {
                var gifts = await _giftService.GetGiftByDonorNameAsync(name);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gifts by donor name {DonorName}", name);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<Gift>>> GetGiftsByTitle(string title)
        {
            try
            {
                var gifts = await _giftService.GetGiftByTitleAsync(title);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gifts by title {Title}", title);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("with-tickets")]
        public async Task<ActionResult<IEnumerable<GetGiftWithTicketsDto>>> GetGiftsWithTickets()
        {
            try
            {
                var gifts = await _giftService.GetGiftsWithTicketsAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving gifts with tickets.");
                return StatusCode(500, "An unexpected error occurred while retrieving gifts with tickets.");
            }
        }

        [HttpGet("max-price")]
        public async Task<IActionResult> GetGiftsWithMaxPrice()
        {
            try
            {
                var gifts = await _giftService.GetGiftsWithMaxPriceAsync();

                if (gifts == null || !gifts.Any())
                    return NotFound("לא נמצאו מתנות");

                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "אירעה שגיאה בעת שליפת מתנות לפי מחיר");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("max-tickets")]
        public async Task<IActionResult> GetGiftsWithMaxTickets()
        {
            try
            {
                var gifts = await _giftService.GetGiftsWithMaxTicketsAsync();

                if (gifts == null || !gifts.Any())
                    return NotFound("לא נמצאו מתנות");

                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "אירעה שגיאה בעת שליפת מתנות לפי מספר כרטיסים");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("with-buyers")]
        public async Task<IActionResult> GetGiftsWithBuyers()
        {
            try
            {
                var gifts = await _giftService.GetGiftsWithBuyersAsync();

                if (gifts == null || !gifts.Any())
                    return NotFound("לא נמצאו מתנות");

                return Ok(gifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת שליפת מתנות עם הרוכשים");
            }
        }
    }
}
