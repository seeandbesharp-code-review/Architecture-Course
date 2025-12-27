using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface ITicketService
    {
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<bool> TicketExistsAsync(int id);

    }
}
