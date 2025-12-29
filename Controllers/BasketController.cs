using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        
        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Basket>>> GetAllBaskets()
        //{
        //    var baskets = await _basketService.GetAllBasketsAsync();
        //    return Ok(baskets);
        //}

        [Authorize]
        [HttpGet("myBasket")]
        public async Task<IActionResult> GetMyBasket()
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));
                var basket = await _basketService.GetBasketByIdAsync(userId);

                if (basket == null)
                    return NotFound("Basket not found.");

                return Ok(basket);
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("user/{userId}")]
        //public async Task<ActionResult<IEnumerable<Basket>>> GetBasketsByUser(int userId)
        //{
        //    var baskets = await _basketService.GetBasketsByUserIdAsync(userId);
        //    return Ok(baskets);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet("gift/{giftId}")]
        //public async Task<ActionResult<IEnumerable<Basket>>> GetBasketsByGift(int giftId)
        //{
        //    var baskets = await _basketService.GetBasketsByGiftIdAsync(giftId);
        //    return Ok(baskets);
        //}

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddBasket(AddBasketDto basket)
        {
            var Id = await _basketService.AddBasketAsync(basket);
            return CreatedAtAction(nameof(GetMyBasket), new { id = Id }, basket);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBasket(int id, UpdateBasketDto basket)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));

                var existingBasket = await _basketService.GetBasketByIdAsync(id);
                if (existingBasket == null)
                    return NotFound("Basket not found.");

                if (existingBasket.UserId != userId)
                    return Forbid("You are not allowed to update this basket.");

                await _basketService.UpdateBasketAsync(id, basket);

                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));

                var existingBasket = await _basketService.GetBasketByIdAsync(id);
                if (existingBasket == null)
                    return NotFound("Basket not found.");

                if (existingBasket.UserId != userId)
                    return Forbid("You are not allowed to delete this basket.");

                await _basketService.DeleteBasketAsync(id);

                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
