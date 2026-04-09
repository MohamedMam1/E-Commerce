using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.Category;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

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

            if (category.Products != null)
            {
                foreach (var product in category.Products)
                {
                    product.IsDeleted = true;
                }
            }

            await _repo.DeleteAsync(category);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _repo.ExistsAsync(id);
        }

        public async Task<PaginatedResultVM<CategoryListVM>> GetFilteredCategoriesAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var query = _repo.GetQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categoryVMs = categories.Select(c => new CategoryListVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products?.Count(p => !p.IsDeleted) ?? 0
            }).ToList();

            return new PaginatedResultVM<CategoryListVM>
            {
                Data = categoryVMs,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
