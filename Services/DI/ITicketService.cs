using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Services.DI
{
    public interface ITicketService
    {
        Task<GetTicketDto> GetTicketByIdAsync(int id);
        Task<IEnumerable<GetTicketDto>> GetAllTicketsAsync();
        Task<int?> AddTicketAsync(AddTicketDto ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<bool> TicketExistsAsync(int id);

    }
}
