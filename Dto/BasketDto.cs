using ChineseRaffleApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChineseRaffleApi.Dto
{
    public class AddBasketDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int Quantity { get; set; } = 1;
    }
    public class UpdateBasketDto
    {
        [Required]
        public int Quantity { get; set; }
    }
    public class GetBasketDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; } 
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

}
