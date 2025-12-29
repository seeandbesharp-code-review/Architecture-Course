using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto user)
        {

            try
            {
                var newId = await _userService.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = newId }, user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
        {
            try
            {
                int userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                                ?? throw new Exception("User ID not found in token"));

                if (id != userIdFromToken)
                    return Forbid("You are not allowed to update another user's data.");

                await _userService.UpdateUserAsync(id, user);
                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                int userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                                ?? throw new Exception("User ID not found in token"));

                if (id != userIdFromToken)
                    return Forbid("You are not allowed to delete another user.");

                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
