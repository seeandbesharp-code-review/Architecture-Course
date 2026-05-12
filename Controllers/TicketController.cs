using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }
        [Authorize]
        [EnableRateLimiting("sliding")]
        [HttpGet("myTickets")]
        public async Task<ActionResult<GetTicketDto>> GetMyTickets()
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID not found in token"));

                var ticket = await _ticketService.GetTicketByIdAsync(userId);
                if (ticket == null)
                    return NotFound("Ticket not found.");

                if (ticket.UserId != userId)
                    return Forbid("You are not allowed to view this ticket.");

                return Ok(ticket);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTickets()
        {
            try
            {
                var tickets = await _ticketService.GetAllTicketsAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tickets.");
                return StatusCode(500, "An unexpected error occurred while retrieving tickets.");
            }
        }


        [Authorize]
        [EnableRateLimiting("sliding")]
        [HttpPost]
        public async Task<ActionResult> CreateTicket(AddTicketDto ticket)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                         ?? throw new Exception("User ID not found in token"));

                ticket.UserId = userId; 

                var Id = await _ticketService.AddTicketAsync(ticket);
                return CreatedAtAction(nameof(GetMyTickets), new { id = Id }, ticket);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateTicket(int id, Ticket ticket)
        //{
        //    if (id != ticket.Id) return BadRequest();
        //    await _ticketService.UpdateTicketAsync(ticket);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteTicket(int id)
        //{
        //    await _ticketService.DeleteTicketAsync(id);
        //    return NoContent();
        //}

    }
}
