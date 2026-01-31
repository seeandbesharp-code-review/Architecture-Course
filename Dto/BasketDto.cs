using ChineseRaffleApi.Models;
//using Microsoft.SqlServer.Management.Smo;
using System.ComponentModel.DataAnnotations;

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

        public string GiftTitle { get; set; } = string.Empty;
        public string GiftImage { get; set; } = string.Empty;
        [Required]
        public decimal GiftTicketPrice { get; set; }
    }

}
