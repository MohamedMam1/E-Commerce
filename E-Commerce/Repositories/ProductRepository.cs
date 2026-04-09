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
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ExtraImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
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
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
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
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.Tag))
                query = query.Where(p => p.Name.Contains(filter.Tag));

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                switch (filter.SortBy)
                {
                    case "price_asc":
                        query = query.OrderBy(p => p.Price);
                        break;

                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                }
            }

            return await query.ToListAsync();
        }

        public IQueryable<Product> GetQueryable()
        {
            return _context.Products
                .Include(p => p.Category)
                .AsQueryable();
        }

        public async Task<(List<Product> Products, int TotalCount)> SearchAndFilterAsync(
            string searchTerm,
            int? categoryId,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == isAvailable.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}