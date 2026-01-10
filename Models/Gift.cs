using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseRaffleApi.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Gift
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; } = 3;
        public Category? Category { get; set; }
        [Required]
        public int DonorId { get; set; }
        public Donor? Donor { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [MaxLength(500)]
        public string? Image { get; set; }
        public int? WinnerId { get; set; }
        public User? Winner { get; set; }
        public IEnumerable<Ticket> TicketList { get; set; } = new List<Ticket>();
    }
}