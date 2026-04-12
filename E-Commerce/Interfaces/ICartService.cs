using E_Commerce.ViewModels.Cart;
using FinalProject.Models;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface ICartService
    {
        Task<CartVM> GetUserCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<(bool Success, string Message)> AddItemToCartAsync(
            string userId, int productId, int quantity, ProductSize size, ProductColor color);
        Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(
            string userId, int productId, int quantity, ProductSize size, ProductColor color);
        Task RemoveItemFromCartAsync(string userId, int productId, ProductSize size, ProductColor color);
        Task ClearCartAsync(string userId);
    }
}
