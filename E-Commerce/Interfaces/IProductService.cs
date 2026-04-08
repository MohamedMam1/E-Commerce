using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.AdminViewModel.Product;
using E_Commerce.ViewModels.Product;

namespace E_Commerce.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListVM>> GetAllProductsAsync();
        Task<ProductDetailVM> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductCreateVM model);
        Task UpdateProductAsync(ProductUpdateVM model);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
        Task<IEnumerable<AdminProductListVM>> GetAdminProductsAsync();
        Task<PaginatedResultVM<AdminProductListVM>> GetFilteredProductsAsync(string searchTerm, int? categoryId, bool? isAvailable, int pageNumber = 1, int pageSize = 10);
    }
}
