using ChineseRaffleApi.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class AddUserDto
    {
        [Required, MaxLength(20)]
        public string UserName { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Password { get; set; } = string.Empty ;
        [EmailAddress, Required]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        [MaxLength(20)]
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
    public class GetUserDto
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
    }
  
}
