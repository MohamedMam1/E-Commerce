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
        Task<IEnumerable<ProductListVM>> SearchProducts(string SearchValue);
        Task<IEnumerable<ProductListVM>> FilterProducts(ProductFilterVM filter);
    }
}
