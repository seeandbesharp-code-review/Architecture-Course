using ChineseRaffleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class AddDonorDto
    {
        [Required, MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
    public class UpdateDonorDto
    {
        [MaxLength(20)]
        public string? Name { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(50), EmailAddress]
        public string? Email { get; set; }
    }
    public class GetDonorDto
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;
        public List<GetGiftForDonorDto> GiftList { get; set; } = new List<GetGiftForDonorDto>();
    }
}
