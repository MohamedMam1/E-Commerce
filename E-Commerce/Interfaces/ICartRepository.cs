using FinalProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCartItemsAsync(string userId);
        Task<Cart> GetCartItemAsync(string userId, int productId);
        Task AddToCartAsync(Cart cartItem);
        Task UpdateCartItemAsync(Cart cartItem);
        Task RemoveFromCartAsync(Cart cartItem);
        Task ClearCartAsync(string userId);
    }
}
