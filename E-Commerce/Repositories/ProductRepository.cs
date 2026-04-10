using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Product;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ITiContext _context;

        public ProductRepository(ITiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && !p.Category.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.ExtraImages)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && !p.Category.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.ExtraImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            product.IsDeleted = false;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            product.IsDeleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products
                .AnyAsync(p => p.Id == id && !p.IsDeleted && !p.Category.IsDeleted);
        }

        public async Task<IEnumerable<Product>> SearchByNameorCat(string SearchValue)
        {
            if (string.IsNullOrWhiteSpace(SearchValue))
                return new List<Product>();

            var lowerSearchValue = SearchValue.Trim().ToLower();

            return await _context.Products
                .Include(p => p.Category)
                .Where(p =>
                    p.Name.ToLower().Contains(lowerSearchValue) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(lowerSearchValue)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterAsync(ProductFilterVM filter)
        public IQueryable<Product> GetQueryable()
        {
            return _context.Products
                .Where(p => !p.IsDeleted && !p.Category.IsDeleted)
                .AsQueryable();
        }

        public async Task DeleteExtraImagesAsync(int productId)
        {
            var extras = _context.ProductImages.Where(pi => pi.ProductId == productId);
            _context.ProductImages.RemoveRange(extras);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<Product> Products, int TotalCount)> SearchAndFilterAsync(string searchTerm,int? categoryId,bool? isAvailable,decimal? minPrice,decimal? maxPrice,int pageNumber = 1,int pageSize = 10)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted && !p.Category.IsDeleted)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            if (isAvailable.HasValue)
            {
                if (isAvailable.Value)
                    query = query.Where(p => p.Quantity > 0);
                else
                    query = query.Where(p => p.Quantity == 0);
            }

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice);

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (products, totalCount);
        }
    }
}