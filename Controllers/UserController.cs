using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("sliding")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with id {id}");
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }




        [EnableRateLimiting("sliding")]
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
                _logger.LogError(ex, "Error adding new user");
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [EnableRateLimiting("sliding")]
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
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with id {id}");
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [EnableRateLimiting("sliding")]
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
                _logger.LogError(ex, "Invalid user ID format.");
                return BadRequest($"Invalid user ID format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with id {id}");  
                return BadRequest(ex.Message);
            }
        }
    }
}
