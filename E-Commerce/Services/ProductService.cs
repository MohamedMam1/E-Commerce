using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.AdminViewModel.Product;
using E_Commerce.ViewModels.Product;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

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
                CategoryName = p.Category?.Name
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

        public async Task<IEnumerable<AdminProductListVM>> GetAdminProductsAsync()
        {
            var products = await _repo.GetAllAsync();
            return products
                .Select(p => new AdminProductListVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category?.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    IsAvailable = p.IsAvailable
                })
                .ToList(); 
        }

        public async Task<PaginatedResultVM<AdminProductListVM>> GetFilteredProductsAsync(string searchTerm, int? categoryId, bool? isAvailable, int pageNumber = 1, int pageSize = 10)
        {
            var query = _repo.GetQueryable()
                .Include(p => p.Category)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == isAvailable.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to view model
            var productVMs = products.Select(p => new AdminProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = p.Quantity,
                IsAvailable = p.IsAvailable
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedResultVM<AdminProductListVM>
            {
                Data = productVMs,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
    }
}
