using ChineseRaffleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class AddGiftDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; } = 3;
        [Required]
        public int DonorId { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [MaxLength(500)]
        public string? Image { get; set; }
    }
    public class AddGiftUploadDto : AddGiftDto
    {
        public IFormFile? ImageFile { get; set; }
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
    public class UpdateGiftUploadDto : UpdateGiftDto
    {
        public IFormFile? ImageFile { get; set; }
    }
    public class GetGiftDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int TicketPrice { get; set; }
        [Required]
        public int DonorId { get; set; }
        public int? CategoryId { get; set; }

        public string? Image { get; set; }
    }
    public class GetGiftWithTicketsDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public IEnumerable<GetTicketDto> Tickets { get; set; } = new List<GetTicketDto>();
        public int QuantitySold { get; set; }
    }
    public class GetGiftWithBuyersDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? Image { get; set; }
        public int QuantitySold { get; set; }
        public IEnumerable<GetUserDto> Buyers { get; set; } = new List<GetUserDto>();
    }
    public class GetGiftForDonorDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
