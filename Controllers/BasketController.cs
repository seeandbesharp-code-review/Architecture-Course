using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basket>>> GetAllBaskets()
        {
            var baskets = await _basketService.GetAllBasketsAsync();
            return Ok(baskets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasketById(int id)
        {
            var basket = await _basketService.GetBasketByIdAsync(id);
            if (basket == null) return NotFound();
            return Ok(basket);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBasketsByUser(int userId)
        {
            var baskets = await _basketService.GetBasketsByUserIdAsync(userId);
            return Ok(baskets);
        }

        [HttpGet("gift/{giftId}")]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBasketsByGift(int giftId)
        {
            var baskets = await _basketService.GetBasketsByGiftIdAsync(giftId);
            return Ok(baskets);
        }

        [HttpPost]
        public async Task<ActionResult> AddBasket(Basket basket)
        {
            await _basketService.AddBasketAsync(basket);
            return CreatedAtAction(nameof(GetBasketById), new { id = basket.Id }, basket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBasket(int id, Basket basket)
        {
            if (id != basket.Id) return BadRequest();

            var exists = await _basketService.BasketExistsAsync(id);
            if (!exists) return NotFound();

            await _basketService.UpdateBasketAsync(basket);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(int id)
        {
            var exists = await _basketService.BasketExistsAsync(id);
            if (!exists) return NotFound();

            await _basketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
