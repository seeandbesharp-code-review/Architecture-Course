using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Models
{
    public enum Role
    {
        Admin,
        User
    }
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string UserName { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string PasswordHash { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
        public ICollection<Ticket>? TicketList { get; set; }
    }
}
