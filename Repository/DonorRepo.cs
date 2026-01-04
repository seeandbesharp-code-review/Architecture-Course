using ChineseRaffleApi.Data;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class DonorRepo: IDonorRepo
    {

        MyContext _context;

        public DonorRepo(MyContext context)
        {
            _context = context;
        }

        public async Task<int> AddDonorAsync(Donor donor)
        {
             _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor.Id;
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor != null)
            {
                _context.Remove(donor);
                await _context.SaveChangesAsync();
                return true;
            }
            return false; 

        }

        public async Task<bool> DonorExistsAsync(string name)
        {
            return await _context.Donors.AnyAsync(donor => donor.Name == name);
        }

        public async Task<IEnumerable<Donor>> GetAllDonorsAsync()
        {
           return await _context.Donors.Include(donor => donor.GiftList).ToListAsync();               
        }

        public async Task<IEnumerable<Donor>> GetDonorByEmailAsync(string email)
        {
            return await _context.Donors.Include(donor => donor.GiftList).Where(donor => donor.Email.Contains(email)).ToListAsync();
        }

        public async Task<Donor?> GetDonorByGiftAsync(int giftId)
        {
            
            var gift = await _context.Gifts
                .Include(g => g.Donor).Include(g => g.Donor.GiftList)
                .FirstOrDefaultAsync(g => g.Id == giftId);
            if (gift != null)
                return gift.Donor;
            else
                return null;
        }
        public async Task<Donor?> GetDonorByIdAsync(int id)
        {
            return await _context.Donors.Include(donor => donor.GiftList).FirstAsync(donor => donor.Id==id);        
        }

        public async Task<IEnumerable<Donor>> GetDonorByNameAsync(string name)
        {
            return await _context.Donors.Where(donor => donor.Name.Contains(name))
                .Include(donor => donor.GiftList)
                .ToListAsync();  
        }

        public async Task<bool> UpdateDonorAsync(int id, Donor updatedDonor)
        {
            var existingDonor = await _context.Donors.FindAsync(id);
            if (existingDonor == null)
                return false;

            existingDonor.Name = updatedDonor.Name;
            existingDonor.Email = updatedDonor.Email;
            existingDonor.PhoneNumber = updatedDonor.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
