using System.ComponentModel.DataAnnotations;

namespace ChineseRaffleApi.Dto
{
    public class CategoryDto
    {
        public class AddCategoryDto
        {
            [Required, MaxLength(20)]
            public string Name { get; set; } = string.Empty;
        }

        public class UpdateCategoryDto
        {
            [MaxLength(20)]
            public string? Name { get; set; }
        }

        public class GetCategoryDto
        {
            [Key]
            public int Id { get; set; }

            [Required, MaxLength(20)]
            public string Name { get; set; } = string.Empty;
        }

        public class GetCategoryWithGiftsDto
        {
            [Key]
            public int Id { get; set; }

            [Required, MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            public ICollection<GetGiftDto>? Gifts { get; set; }
        }
    }
}
