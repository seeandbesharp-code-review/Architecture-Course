using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Services.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ChineseRaffleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate user and get JWT token
        /// </summary>
        [EnableRateLimiting("sliding")]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.UserName) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                _logger.LogWarning("Login attempt with missing credentials.");
                return BadRequest(new { message = "userName and password are required." });
            }
            var result = await _userService.AuthenticateAsync(loginDto.UserName, loginDto.Password);

            if (result == null)
            {
                _logger.LogWarning("Invalid login attempt for user: {UserName}", loginDto.UserName);
                return Unauthorized(new { message = "Invalid userName or password." });
            }
            return Ok(result);
        }

        [EnableRateLimiting("sliding")]
        [HttpPost("register")]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserDto>> Register([FromBody] AddUserDto createDto)
        {
            try
            {
                var Id = await _userService.AddUserAsync(createDto);
                return CreatedAtAction(nameof(Register), new { id = Id }, createDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Registration failed: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred during registration: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
