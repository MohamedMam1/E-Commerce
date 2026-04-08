using FinalProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetWishlistItemsAsync(string userId);
        Task<Wishlist> GetWishlistItemAsync(string userId, int productId);
        Task AddToWishlistAsync(Wishlist item);
        Task RemoveFromWishlistAsync(Wishlist item);
        Task ClearWishlistAsync(string userId);
    }
}
