using ChineseRaffleApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers.DI
{
    public interface ITicketController
    {
        Task<ActionResult<Ticket>> GetTicket(int id);
        Task<ActionResult<IEnumerable<Ticket>>> GetTickets();
        Task<ActionResult> CreateTicket(Ticket ticket);
        Task<ActionResult> UpdateTicket(int id, Ticket ticket);
        Task<ActionResult> DeleteTicket(int id);

    }
}
