using ChineseRaffleApi.Data;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class UserRepo :IUserRepo
    {
        MyContext _context;

        public UserRepo(MyContext context)
        {
            _context = context;
        }

        public MyContext Get_context()
        {
            return _context;
        }

        public async Task<int> AddUserAsync(User user)
        {
    
            _context.Users.Add(user);
          
           await _context.SaveChangesAsync();
           return user.Id; 
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        //public async Task UpdateUserAsync(int id, User user)
        //{
        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<bool> UpdateUserAsync(int id ,User updatedUser)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return false; 
                }

                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }
        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }   
    }
}
