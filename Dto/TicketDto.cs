using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class AddTicketDto
    {
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
    public class GetTicketDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string GiftTitle { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}
