using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDto>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();
            return ticket;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(AddTicketDto ticket)
        {
            var Id = await _ticketService.AddTicketAsync(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id) return BadRequest();
            await _ticketService.UpdateTicketAsync(ticket);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }

    }
}
