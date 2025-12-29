using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using System.Net.Sockets;

namespace ChineseRaffleApi.Services
{
    public class TicketService: ITicketService
    {
        private readonly ITicketRepo _ticketRepo;
        private readonly IMapper _mapper;


        public TicketService(ITicketRepo ticketRepo, IMapper mapper)
        {
            _ticketRepo = ticketRepo;
            _mapper = mapper;
        }

        public async Task<GetTicketDto> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepo.GetTicketByIdAsync(id);
            return _mapper.Map<GetTicketDto>(ticket);
        }

        public async Task<IEnumerable<GetTicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepo.GetAllTicketsAsync();
            return _mapper.Map<IEnumerable<GetTicketDto>>(tickets);
        }

        public async Task<int?> AddTicketAsync(AddTicketDto ticket)
        {
            var MapTickets = _mapper.Map<Ticket>(ticket);
            return await _ticketRepo.AddTicketAsync(MapTickets);
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
