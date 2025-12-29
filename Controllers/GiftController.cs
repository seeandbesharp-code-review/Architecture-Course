using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftController(IGiftService giftService)
        {
            _giftService = giftService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gift>> GetGift(int id)
        {
            var gift = await _giftService.GetGiftByIdAsync(id);
            if (gift == null)
            {
                return NotFound();
            }
            return Ok(gift);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gift>>> GetAllGifts()
        {
            var gifts = await _giftService.GetAllGiftsAsync();
            return Ok(gifts);
        }

        [HttpPost]
        public async Task<ActionResult<Gift>> AddGift([FromBody] AddGiftDto gift)
        {
            try
            {
                var Id = await _giftService.AddGiftAsync(gift);
                return CreatedAtAction(nameof(GetGift), new { id = Id }, gift);
            }
            catch (ArgumentException ex)
            {
                // Duplicate title or other validation from service -> return 409 Conflict with message
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGift(int id, [FromBody] UpdateGiftDto gift)
        {
            try
            {
                await _giftService.UpdateGiftAsync(id, gift);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Duplicate title (or business-rule) -> 409 Conflict
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            var isDeleted = await _giftService.DeleteGiftAsync(id);
            if (isDeleted)
            {
                return Ok($"Gift id:{id} was deleted");
            }
            return NotFound($"Gift id:{id} not found");

        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> GiftExists(string title)
        {
            var exists = await _giftService.GiftExistsAsync(title);
            return Ok(exists);
        }
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
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("with-tickets")]
        public async Task<ActionResult<IEnumerable<GetGiftWithTicketsDto>>> GetGiftsWithTickets()
        {
            var gifts = await _giftService.GetGiftsWithTicketsAsync();
            return Ok(gifts);
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
                return StatusCode(500, "אירעה שגיאה בעת שליפת מתנות לפי מחיר");
            }
        }

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
                return StatusCode(500, "אירעה שגיאה בעת שליפת מתנות לפי מספר כרטיסים");
            }
        }
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
