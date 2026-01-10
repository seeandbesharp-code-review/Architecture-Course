using ChineseRaffleApi.Data;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using Microsoft.EntityFrameworkCore;

namespace ChineseRaffleApi.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly MyContext _context;

        public CategoryRepo(MyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetCategoryWithGiftsAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Gifts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesWithGiftsAsync()
        {
            return await _context.Categories
                .Include(c => c.Gifts)
                .ToListAsync();
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category updatedCategory)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null) return false;

            existingCategory.Name = updatedCategory.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> CategoryExistsAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }
        public async Task<bool> HasGiftsAsync(int categoryId)
        {
            return await _context.Gifts.AnyAsync(g => g.CategoryId == categoryId);
        }
    }
}
