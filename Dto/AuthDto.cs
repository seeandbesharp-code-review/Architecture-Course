using ChineseRaffleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
       
        public string TokenType { get; set; } = "Bearer";
        [Required]
        public int ExpiresIn { get; set; }
        public GetUserDto User { get; set; } = null!;
    }
}


