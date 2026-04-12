using E_Commerce.ViewModels.Cart;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface ICartService
    {
        Task<CartVM> GetUserCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<(bool Success, string Message)> AddItemToCartAsync(
            string userId, int productVariantId, int quantity);
        Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(
            string userId, int cartItemId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
    }
}

