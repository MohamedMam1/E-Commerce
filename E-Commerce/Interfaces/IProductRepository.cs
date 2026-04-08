using E_Commerce.ViewModels.Product;
using FinalProject.Models;

namespace E_Commerce.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Product>> SearchByNameorCat(string SearchValue);
        Task<IEnumerable<Product>> FilterAsync(ProductFilterVM filter);
        IQueryable<Product> GetQueryable();
        Task<(List<Product> Products, int TotalCount)> SearchAndFilterAsync(
            string searchTerm,
            int? categoryId,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice,
            int pageNumber = 1,
            int pageSize = 10);
    }
}
