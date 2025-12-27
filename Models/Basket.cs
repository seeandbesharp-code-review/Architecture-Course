using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseRaffleApi.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        public Gift? Gift { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}
