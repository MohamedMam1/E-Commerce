using E_Commerce.Interfaces;
using E_Commerce.Models;
using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.AdminViewModel.Product;
using E_Commerce.ViewModels.Product;
using FinalProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IProductRepository repo, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
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
                productImages = p.ExtraImages?.Select(img => img.ImageUrl).ToList(),
                Variants = p.ProductVariants?.Select(v => new ProductVariantVM
                {
                    Id = v.Id,             
                    Size = v.Size,
                    Color = v.Color,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariantVM>()
            };
        }

        public async Task<ProductEditVM> GetProductEditVMAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductEditVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ExistingMainImageUrl = p.ImageUrl,
                ExistingExtraImageUrls = p.ExtraImages?
                    .Select(ei => ei.ImageUrl)
                    .ToList() ?? new List<string>(),
                Variants = p.ProductVariants?.Select(v => new ProductVariantVM
                {
                    Id = v.Id,             
                    Size = v.Size,
                    Color = v.Color,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariantVM>()
            };
        }

        public async Task AddProductAsync(ProductCreateVM model)
        {
            // Save main image
            var mainImageUrl = await SaveImageAsync(model.MainImage);

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                ImageUrl = mainImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            // Save extra images
            var extraImages = new List<ProductImage>();
            var extraFiles = new[] { model.ExtraImage1, model.ExtraImage2, model.ExtraImage3 };

            foreach (var file in extraFiles)
            {
                if (file != null && file.Length > 0)
                {
                    var url = await SaveImageAsync(file);
                    extraImages.Add(new ProductImage { ImageUrl = url });
                }
            }

            product.ExtraImages = extraImages;

            // Add variants
            product.ProductVariants = model.Variants?.Select(v => new ProductVariant
            {
                Size = v.Size,
                Color = v.Color,
                Stock = v.Stock,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? new List<ProductVariant>();

            await _repo.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductEditVM model)
        {
            var product = await _repo.GetByIdAsync(model.Id);
            if (product == null) throw new Exception("Product not found");

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;

            // Replace main image only if a new one is uploaded
            if (model.MainImage != null && model.MainImage.Length > 0)
            {
                DeleteImageFile(product.ImageUrl);
                product.ImageUrl = await SaveImageAsync(model.MainImage);
            }

            // Replace extra images only if any new ones are uploaded
            var extraFiles = new[] { model.ExtraImage1, model.ExtraImage2, model.ExtraImage3 }
                .Where(f => f != null && f.Length > 0)
                .ToList();

            if (extraFiles.Any())
            {
                // Delete old extra images from disk
                if (product.ExtraImages != null)
                    foreach (var ei in product.ExtraImages)
                        DeleteImageFile(ei.ImageUrl);

                // Remove old records from DB
                await _repo.DeleteExtraImagesAsync(product.Id);

                // Save new ones
                product.ExtraImages = new List<ProductImage>();
                foreach (var file in extraFiles)
                {
                    var url = await SaveImageAsync(file);
                    product.ExtraImages.Add(new ProductImage { ImageUrl = url });
                }
            }

            // Update variants
            if (model.Variants != null && model.Variants.Any())
            {
                await _repo.DeleteVariantsAsync(product.Id);
                product.ProductVariants = model.Variants.Select(v => new ProductVariant
                {
                    Size = v.Size,
                    Color = v.Color,
                    Stock = v.Stock,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
            }

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
            return products.Select(p => new AdminProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = p.ProductVariants?.Sum(v => v.Stock) ?? 0,
                IsAvailable = p.ProductVariants?.Any(v => v.Stock > 0) ?? false
            }).ToList();
        }

        public async Task<PaginatedResultVM<AdminProductListVM>> GetFilteredProductsAsync(string searchTerm, int? categoryId, bool? isAvailable,int pageNumber = 1, int pageSize = 10)
        {
            var query = _repo.GetQueryable()
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (isAvailable.HasValue)
            {
                if (isAvailable.Value)
                    query = query.Where(p => p.ProductVariants.Any(v => v.Stock > 0));
                else
                    query = query.Where(p => !p.ProductVariants.Any(v => v.Stock > 0));
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productVMs = products.Select(p => new AdminProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = p.ProductVariants?.Sum(v => v.Stock) ?? 0,
                IsAvailable = p.ProductVariants?.Any(v => v.Stock > 0) ?? false
            }).ToList();

            return new PaginatedResultVM<AdminProductListVM>
            {
                Data = productVMs,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        // ── Private Helpers ───────────────────────────────────────────────

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/products/{uniqueFileName}";
        }

        private void DeleteImageFile(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }



        public async Task<PaginatedResultVM<ProductListVM>> GetFilteredProductsForCustomerAsync(string searchTerm,string categoryName,int? categoryId,bool? isAvailable,
               decimal? minPrice,decimal? maxPrice,string sortBy,int pageNumber = 1,int pageSize = 12)
        {
            var query = _repo.GetQueryable()
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm) ||
                                         p.Description.Contains(searchTerm));

            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(p => p.Category != null && p.Category.Name == categoryName);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (isAvailable.HasValue)
            {
                if (isAvailable.Value)
                    query = query.Where(p => p.ProductVariants.Any(v => v.Stock > 0));
                else
                    query = query.Where(p => !p.ProductVariants.Any(v => v.Stock > 0));
            }

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productVMs = products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = p.ProductVariants?.Sum(v => v.Stock) ?? 0,
                IsAvailable = p.ProductVariants?.Any(v => v.Stock > 0) ?? false
            }).ToList();

            return new PaginatedResultVM<ProductListVM>
            {
                Data = productVMs,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}