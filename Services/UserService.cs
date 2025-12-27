using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;

namespace ChineseRaffleApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<int> AddUserAsync(AddUserDto user)
        {
            var trimmedUserName = user.UserName?.Trim() ?? string.Empty;
            if (await _userRepo.UserExistsAsync(trimmedUserName))
            {
                throw new ArgumentException($"UserName '{trimmedUserName}' is already registered.");
            }

            User newUser = new User()
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.Password,
                PhoneNumber = user.PhoneNumber,
            };
            return await _userRepo.AddUserAsync(newUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepo.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetUserByIdAsync(id);
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto user)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser != null)
            {
                if (user.UserName != null && user.UserName != existingUser.UserName)
                {
                    if (await _userRepo.UserExistsAsync(user.UserName))
                    {
                        throw new ArgumentException($"UserName {user.UserName} is already registered.");
                    }
                    existingUser.UserName = user.UserName;
                }
                if (user.Email != null) existingUser.Email = user.Email;
                if (user.PhoneNumber != null) existingUser.PhoneNumber = user.PhoneNumber;
                await _userRepo.UpdateUserAsync(id, existingUser);
            }
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepo.UserExistsAsync(username);
        }
    }
}
