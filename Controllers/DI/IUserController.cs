using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers.DI
{
    public interface IUserController
    {
        public Task<IActionResult> GetAllUsers();
        public Task<IActionResult> GetUserById(int id);
        public Task<IActionResult> AddUser([FromBody] AddUserDto user);
        public Task<IActionResult> UpdateUser(int id, [FromBody] User user);
        public Task<IActionResult> DeleteUser(int id);

    }
}
