using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Repository.DI
{
    public interface ITicketRepo
    {
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<int?> AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<bool> TicketExistsAsync(int id);

    }
}
