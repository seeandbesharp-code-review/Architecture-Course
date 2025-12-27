using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;

namespace ChineseRaffleApi.Services.DI
{
    public interface IUserService
    {
        Task<int> AddUserAsync(AddUserDto user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, UpdateUserDto user);
        Task<bool> UserExistsAsync(string username);
    }
}
