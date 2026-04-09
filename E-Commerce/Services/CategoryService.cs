using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Category;
using E_Commerce.ViewModels.AdminDashboard;
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

        // 🔹 Get all categories (optimized)
        public async Task<IEnumerable<CategoryListVM>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetQueryable()
                .Include(c => c.Products)
                .ToListAsync();

            return categories.Select(c => new CategoryListVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count(p => !p.IsDeleted)
            });
        }

        // 🔹 Get category by id
        public async Task<CategoryDetailVM> GetCategoryByIdAsync(int id)
        {
            var c = await _repo.GetQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return null;

            return new CategoryDetailVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            };
        }

        // 🔹 Add category
        public async Task AddCategoryAsync(CategoryCreateVM model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            await _repo.AddAsync(category);
        }

        // 🔹 Update category
        public async Task UpdateCategoryAsync(CategoryEditVM model)
        {
            var category = await _repo.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (category == null)
                throw new KeyNotFoundException("Category not found");

            category.Name = model.Name;
            category.Description = model.Description;

            await _repo.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repo.GetQueryable()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException("Category not found");

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

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await _repo.IsNameExists(name);
        }

        public async Task<PaginatedResultVM<CategoryListVM>> GetFilteredCategoriesAsync(
            string searchTerm,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _repo.GetQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.Name.Contains(searchTerm) ||
                    (c.Description != null && c.Description.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryListVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ProductCount = c.Products.Count(p => !p.IsDeleted)
                })
                .ToListAsync();

            return new PaginatedResultVM<CategoryListVM>
            {
                Data = categories,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}