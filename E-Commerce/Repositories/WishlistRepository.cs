using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ITiContext _context;

        public WishlistRepository(ITiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistItemsAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wishlist> GetWishlistItemAsync(string userId, int productId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task AddToWishlistAsync(Wishlist item)
        {
            await _context.Wishlists.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWishlistAsync(Wishlist item)
        {
            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task ClearWishlistAsync(string userId)
        {
            var userWishlist = await GetWishlistItemsAsync(userId);
            _context.Wishlists.RemoveRange(userWishlist);
            await _context.SaveChangesAsync();
        }
    }
}
