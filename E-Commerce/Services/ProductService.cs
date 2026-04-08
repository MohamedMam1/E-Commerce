using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Product;
using FinalProject.Models;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductListVM>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            });
        }

        public async Task<ProductDetailVM> GetProductByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductDetailVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                productImages = p.ExtraImages?.Select(img => img.ImageUrl).ToList()
            };
        }

        public async Task AddProductAsync(ProductCreateVM model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                IsAvailable = model.IsAvailable,
                CategoryId = model.CategoryId
            };

            await _repo.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductUpdateVM model)
        {
            var product = await _repo.GetByIdAsync(model.Id);
            if (product == null) throw new Exception("Product not found");

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.IsAvailable = model.IsAvailable;
            product.CategoryId = model.CategoryId;

            await _repo.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            await _repo.DeleteAsync(product);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _repo.ExistsAsync(id);
        }

        public async Task<IEnumerable<ProductListVM>> SearchProducts(string SearchValue)
        {
            var products = await _repo.SearchByNameorCat(SearchValue);

            return products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            });
        }

        public async Task<IEnumerable<ProductListVM>> FilterProducts(ProductFilterVM filter)
        {
            var products = await _repo.FilterAsync(filter);

            return products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            });
        }
    }
}
