using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Repository.DI
{
    public interface IUserRepo
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        //Task<User> GetUserByUsernameAsync(string username);
        Task<int> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string username);
        Task<User?> GetUserByUserNameAsync(string username);

    }
}
