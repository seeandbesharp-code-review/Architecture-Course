using AutoMapper;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using static ChineseRaffleApi.Dto.CategoryDto;

namespace ChineseRaffleApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        }

        public async Task<GetCategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            return _mapper.Map<GetCategoryDto>(category);
        }

        public async Task<GetCategoryWithGiftsDto?> GetCategoryWithGiftsAsync(int id)
        {
            var category = await _categoryRepo.GetCategoryWithGiftsAsync(id);
            return _mapper.Map<GetCategoryWithGiftsDto>(category);
        }
        public async Task<IEnumerable<GetCategoryWithGiftsDto>> GetAllCategoriesWithGiftsAsync()
        {
            var category = await _categoryRepo.GetAllCategoriesWithGiftsAsync();
            return _mapper.Map<IEnumerable<GetCategoryWithGiftsDto>>(category);
        }


        public async Task<int> AddCategoryAsync(AddCategoryDto categoryDto)
        {
            if (await _categoryRepo.CategoryExistsAsync(categoryDto.Name))
            {
                throw new ArgumentException($"Category '{categoryDto.Name}' already exists.");
            }

            var category = new Category { Name = categoryDto.Name };
            return await _categoryRepo.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found");

            if (!string.IsNullOrEmpty(categoryDto.Name))
            {
                if (categoryDto.Name != category.Name && await _categoryRepo.CategoryExistsAsync(categoryDto.Name))
                {
                    throw new ArgumentException($"Category '{categoryDto.Name}' already exists.");
                }
                category.Name = categoryDto.Name;
            }

            await _categoryRepo.UpdateCategoryAsync(id, category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (await _categoryRepo.HasGiftsAsync(id))
            {
                throw new InvalidOperationException("This category contains gifts, you must first delete the gifts in the category.");
            }
            await _categoryRepo.DeleteCategoryAsync(id);
        }
    }
}