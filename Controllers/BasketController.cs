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
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketService basketService, ILogger<BasketController> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }


        [Authorize]
        [HttpGet("myBasket")]
        public async Task<IActionResult> GetMyBasket()
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));
                var basket = await _basketService.GetBasketsByUserIdAsync(userId);

                if (basket == null)
                    return NotFound("Basket not found.");

                return Ok(basket);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"{ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddBasket(AddBasketDto basket)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                           ?? throw new Exception("User ID not found in token"));

                if (basket.UserId != userId)
                    return Forbid("You are not allowed to add a basket for another user.");

                var Id = await _basketService.AddBasketAsync(basket);
                return CreatedAtAction(nameof(GetMyBasket), new { id = Id }, basket);
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Invalid UserId format in token.");
                return BadRequest("Invalid user ID in token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a basket.");
                return StatusCode(500, "An unexpected error occurred while adding the basket.");
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBasket(int id, UpdateBasketDto basket)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));

                await _basketService.UpdateBasketAsync(id, basket);

                return NoContent();
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating basket.");
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
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting basket.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPost("buy")]
        public async Task<ActionResult> BuyTicketsFromBasket()
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));

                await _basketService.BuyTicketsFromBasket(userId);

                return NoContent();
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while buying tickets from basket.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
