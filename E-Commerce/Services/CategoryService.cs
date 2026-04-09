using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Category;
using FinalProject.Models;

namespace E_Commerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryListVM>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryListVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products?.Count(p => !p.IsDeleted) ?? 0
            });
        }

        public async Task<CategoryDetailVM> GetCategoryByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new CategoryDetailVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            };
        }

        public async Task AddCategoryAsync(CategoryCreateVM model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            await _repo.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryEditVM model)
        {
            var category = await _repo.GetByIdAsync(model.Id);
            if (category == null) throw new Exception("Category not found");

            category.Name = model.Name;
            category.Description = model.Description;

            await _repo.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) throw new Exception("Category not found");

            await _repo.DeleteAsync(category);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _repo.ExistsAsync(id);
        }
    }
}
