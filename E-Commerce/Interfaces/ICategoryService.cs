using E_Commerce.ViewModels.Category;

namespace E_Commerce.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListVM>> GetAllCategoriesAsync();
        Task<CategoryDetailVM> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryCreateVM model);
        Task UpdateCategoryAsync(CategoryEditVM model);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(int id);
    }
}
