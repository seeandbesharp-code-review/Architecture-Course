using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class TicketService: ITicketService
    {
        private readonly ITicketRepo _ticketRepo;

        public TicketService(ITicketRepo ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _ticketRepo.GetTicketByIdAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepo.GetAllTicketsAsync();
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _ticketRepo.AddTicketAsync(ticket);
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            await _ticketRepo.UpdateTicketAsync(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _ticketRepo.DeleteTicketAsync(id);
        }

        public async Task<bool> TicketExistsAsync(int id)
        {
            return await _ticketRepo.TicketExistsAsync(id);
        }
    }
}
