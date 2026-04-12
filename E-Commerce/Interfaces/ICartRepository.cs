using FinalProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task<CartItem> GetCartItemAsync(string userId, int productVariantId);
        Task AddToCartAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync(string userId);
    }
}

