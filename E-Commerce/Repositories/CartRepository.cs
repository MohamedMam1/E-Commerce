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

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.ProductVariant)
                .ThenInclude(pv => pv.Product)
                .ToListAsync();
        }

        public async Task<CartItem> GetCartItemAsync(string userId, int productVariantId)
        {
            return await _context.CartItems
                .Include(c => c.ProductVariant)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId && c.ProductVariantId == productVariantId);
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var userCart = await GetCartItemsAsync(userId);
            _context.CartItems.RemoveRange(userCart);
            await _context.SaveChangesAsync();
        }
    }
}

