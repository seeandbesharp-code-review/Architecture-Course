using Azure.Messaging;
using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Donor>> GetDonor(int id)
        {
            var donor = await _donorService.GetDonorByIdAsync(id);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donor>>> GetAllDonors()
        {
            var donors = await _donorService.GetAllDonorsAsync();
            return Ok(donors);
        }

        [HttpPost]
        public async Task<ActionResult<Donor>> AddDonor([FromBody] AddDonorDto donor)
        {
            var Id = await _donorService.AddDonorAsync(donor);
            return CreatedAtAction(nameof(GetDonor), new { id = Id }, donor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromBody] UpdateDonorDto donor)
        {
            try
            {
                await _donorService.UpdateDonorAsync(id, donor);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
           
            var isDeleted = await _donorService.DeleteDonorAsync(id);
            if (isDeleted)
            {
                return Ok($"donor id:{id} was deleted");
            }
            return NotFound($"Donor id:{id} not found");
        }

        [HttpGet("exists/{name}")]
        public async Task<ActionResult<bool>> DonorExists(string name)
        {
            var exists = await _donorService.DonorExistsAsync(name);
            return Ok(exists);
        }


        [HttpGet("byName/{name}")]
        public async Task<ActionResult<IEnumerable<Donor>>> GetDonorByName(string name)
        {
            var donors = await _donorService.GetDonorByNameAsync(name);
            return Ok(donors);       
        }
        [HttpGet("byEmail/{email}")]
        public async Task<ActionResult<IEnumerable<Donor>>> GetDonorByEmail(string email)
        {
            var donors = await _donorService.GetDonorByEmailAsync(email);
            return Ok(donors);
        }

        [HttpGet("byGift/{giftId}")]
        public async Task<ActionResult<Donor>> GetDonorByGift(int giftId)
        {
            var donor = await _donorService.GetDonorByGiftAsync(giftId);
            return Ok(donor);
        }
    }
}
