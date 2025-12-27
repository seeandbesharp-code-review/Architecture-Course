using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Models
{
    public class Donor
    {
        [Key]
        public int Id { get; set; }
        [Required ,MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty ;
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty ;
        public ICollection<Gift>? GiftList { get; set; }
    }
}
