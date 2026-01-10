using ChineseRaffleApi.Models;

namespace ChineseRaffleApi.Repository.DI
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryWithGiftsAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(string name);
        Task<bool> HasGiftsAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesWithGiftsAsync();
    }
}
