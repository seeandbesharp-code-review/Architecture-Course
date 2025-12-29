using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using Microsoft.Extensions.Configuration;

namespace ChineseRaffleApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;



        public UserService(IUserRepo userRepo, ITokenService tokenService, IConfiguration configuration
            , ILogger<UserService> logger, IMapper mapper)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
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
                PasswordHash = HashPassword(user.Password),
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
        public async Task<LoginResponseDto?> AuthenticateAsync(string userName, string password)
        {
            var user = await _userRepo.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User not found for userName {userName}", userName);
                return null;
            }

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for userName {userName}", userName);
                return null;
            }

            var token = _tokenService.GenerateToken(user.Id, user.Email, user.UserName, user.Role);
            var expiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 60);

            _logger.LogInformation("User {UserId} authenticated successfully", user.Id);

            return new LoginResponseDto
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60, // Convert to seconds
                User = _mapper.Map<GetUserDto>(user)
            };
        }
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
