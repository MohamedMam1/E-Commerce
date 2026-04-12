using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Wishlist;
using FinalProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepo;

        public WishlistService(IWishlistRepository wishlistRepo)
        {
            _wishlistRepo = wishlistRepo;
        }

        public async Task<WishlistVM> GetUserWishlistAsync(string userId)
        {
            var items = await _wishlistRepo.GetWishlistItemsAsync(userId);

            var vm = new WishlistVM
            {
                Items = items.Select(w => new WishlistItemVM
                {
                    ProductId = w.ProductId,
                    ProductName = w.Product?.Name ?? "Unknown Product",
                    Price = w.Product?.Price ?? 0,
                    ImageUrl = w.Product?.ImageUrl,
                    IsAvailable = w.Product?.IsAvailable ?? false,
                    Size = w.Product?.Size ?? default,
                    Color = w.Product?.Color ?? default
                }).ToList()
            };

            return vm;
        }

        public async Task<int> GetWishlistItemCountAsync(string userId)
        {
            var items = await _wishlistRepo.GetWishlistItemsAsync(userId);
            return items.Count();
        }

        public async Task ToggleWishlistItemAsync(string userId, int productId)
        {
            var existingItem = await _wishlistRepo.GetWishlistItemAsync(userId, productId);

            if (existingItem != null)
            {
                await _wishlistRepo.RemoveFromWishlistAsync(existingItem);
            }
            else
            {
                var newItem = new Wishlist
                {
                    UserId = userId,
                    ProductId = productId
                };
                await _wishlistRepo.AddToWishlistAsync(newItem);
            }
        }

        public async Task RemoveItemFromWishlistAsync(string userId, int productId)
        {
            var existingItem = await _wishlistRepo.GetWishlistItemAsync(userId, productId);
            if (existingItem != null)
            {
                await _wishlistRepo.RemoveFromWishlistAsync(existingItem);
            }
        }

        public async Task ClearWishlistAsync(string userId)
        {
            await _wishlistRepo.ClearWishlistAsync(userId);
        }
    }
}
