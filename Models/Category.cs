using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Gift>? Gifts { get; set; }
    }
}