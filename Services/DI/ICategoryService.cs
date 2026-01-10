using ChineseRaffleApi.Dto;
using static ChineseRaffleApi.Dto.CategoryDto;

namespace ChineseRaffleApi.Services.DI
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync();
        Task<GetCategoryDto?> GetCategoryByIdAsync(int id);
        Task<GetCategoryWithGiftsDto?> GetCategoryWithGiftsAsync(int id);
        Task<int> AddCategoryAsync(AddCategoryDto categoryDto);
        Task UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<GetCategoryWithGiftsDto>> GetAllCategoriesWithGiftsAsync();
    }
}
