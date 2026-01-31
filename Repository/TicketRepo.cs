using ChineseRaffleApi.Data;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class TicketRepo : ITicketRepo
    {
        MyContext _context;

        public TicketRepo(MyContext context)
        {
            _context = context;
        }
        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.Include(t => t.Gift).Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.Include(t => t.Gift).Include(t => t.User).ToListAsync();
        }

        public async Task<int?> AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // כאן תראי את השגיאה האמיתית מה-SQL Server
                var innerException = ex.InnerException?.Message;
                throw new Exception(innerException);
            }
            return ticket.Id;   
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await GetTicketByIdAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TicketExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.Id == id);
        }
    }
}
