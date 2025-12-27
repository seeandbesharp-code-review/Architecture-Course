using ChineseRaffleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class AddGiftDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        [Required]
        public int DonorId { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [MaxLength(500)]
        public string? Image { get; set; }
    }
    public class UpdateGiftDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }
        public int? CategoryId { get; set; }
        public int? DonorId { get; set; }
        public int? TicketPrice { get; set; }
        [MaxLength(500)]
        public string? Image { get; set; }
        public int? WinnerId { get; set; }
    }
    public class GetGiftDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        [Required]
        public string DonorName { get; set; } = string.Empty;
        public int TicketPrice { get; set; }
        public string? Image { get; set; }
        public string? WinnerName { get; set; }
    }
}
