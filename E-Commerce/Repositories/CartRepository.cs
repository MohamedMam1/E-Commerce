using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ITiContext _context;

        public CartRepository(ITiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetCartItemsAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart> GetCartItemAsync(string userId, int productId, ProductSize size, ProductColor color)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId && c.ProductId == productId && c.Size == size && c.Color == color);
        }

        public async Task AddToCartAsync(Cart cartItem)
        {
            await _context.Carts.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(Cart cartItem)
        {
            _context.Carts.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(Cart cartItem)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var userCart = await GetCartItemsAsync(userId);
            _context.Carts.RemoveRange(userCart);
            await _context.SaveChangesAsync();
        }
    }
}
