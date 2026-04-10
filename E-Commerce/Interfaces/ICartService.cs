using E_Commerce.ViewModels.Cart;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface ICartService
    {
        Task<CartVM> GetUserCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task AddItemToCartAsync(string userId, int productId, int quantity = 1);
        Task UpdateCartItemQuantityAsync(string userId, int productId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
    }
}
