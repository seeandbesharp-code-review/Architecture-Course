using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly ILogger<DonorController> _logger;

       public DonorController(IDonorService donorService, ILogger<DonorController> logger)
        {
            _donorService = donorService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDonorDto>> GetDonor(int id)
        {
            try
            {
                var donor = await _donorService.GetDonorByIdAsync(id);
                if (donor == null)
                    return NotFound($"Donor with id {id} not found");

                return Ok(donor);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Error retrieving donor with id {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDonorDto>>> GetAllDonors()
        {
            try
            {
                var donors = await _donorService.GetAllDonorsAsync();
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all donors");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddDonor([FromBody] AddDonorDto donor)
        {
            try
            {
                var id = await _donorService.AddDonorAsync(donor);
                return CreatedAtAction(nameof(GetDonor), new { id }, donor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new donor");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromBody] UpdateDonorDto donor)
        {
            try
            {
                await _donorService.UpdateDonorAsync(id, donor);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor with id {id} not found for update");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating donor with id {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            try
            {
                var deleted = await _donorService.DeleteDonorAsync(id);
                if (!deleted)
                    return NotFound($"Donor id {id} not found");

                return Ok($"Donor id {id} was deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting donor with id {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exists/{name}")]
        public async Task<ActionResult<bool>> DonorExists(string name)
        {
            try
            {
                var exists = await _donorService.DonorExistsAsync(name);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of donor with name {name}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<IEnumerable<GetDonorDto>>> GetDonorByName(string name)
        {
            try
            {
                var donors = await _donorService.GetDonorByNameAsync(name);
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving donors with name {name}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("byEmail/{email}")]
        public async Task<ActionResult<IEnumerable<GetDonorDto>>> GetDonorByEmail(string email)
        {
            try
            {
                var donors = await _donorService.GetDonorByEmailAsync(email);
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving donors with email {email}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("byGift/{giftId}")]
        public async Task<ActionResult<GetDonorDto>> GetDonorByGift(int giftId)
        {
            try
            {
                var donor = await _donorService.GetDonorByGiftAsync(giftId);
                if (donor == null)
                    return NotFound($"No donor found for gift id {giftId}");

                return Ok(donor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving donor for gift id {giftId}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
