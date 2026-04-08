using E_Commerce.ViewModels.Wishlist;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistVM> GetUserWishlistAsync(string userId);
        Task<int> GetWishlistItemCountAsync(string userId);
        Task ToggleWishlistItemAsync(string userId, int productId);
        Task RemoveItemFromWishlistAsync(string userId, int productId);
        Task ClearWishlistAsync(string userId);
    }
}
