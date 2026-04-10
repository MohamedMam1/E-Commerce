using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ITiContext _context;

        public CategoryRepository(ITiContext context)
        {
            _context = context;
        }
        public async Task<bool> IsNameExists(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.Trim().ToLower());
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public IQueryable<Category> GetQueryable()
        {
            return _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Products)
                .AsQueryable();
        }

        public async Task AddAsync(Category category)
        {
            category.IsDeleted = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            category.IsDeleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories
                .AnyAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}
