using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Cart;
using FinalProject.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;

        public CartService(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<CartVM> GetUserCartAsync(string userId)
        {
            var cartItems = await _cartRepo.GetCartItemsAsync(userId);

            var vm = new CartVM
            {
                Items = cartItems.Select(c => new CartItemVM
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product?.Name ?? "Unknown Product",
                    Price = c.Product?.Price ?? 0,
                    ImageUrl = c.Product?.ImageUrl,
                    Quantity = c.Quantity
                }).ToList()
            };

            return vm;
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cartItems = await _cartRepo.GetCartItemsAsync(userId);
            return cartItems.Sum(c => c.Quantity);
        }

        public async Task AddItemToCartAsync(string userId, int productId, int quantity = 1)
        {
            if (quantity <= 0) return;

            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _cartRepo.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepo.AddToCartAsync(newItem);
            }
        }

        public async Task UpdateCartItemQuantityAsync(string userId, int productId, int quantity)
        {
            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId);
            if (existingItem != null)
            {
                if (quantity <= 0)
                {
                    await _cartRepo.RemoveFromCartAsync(existingItem);
                }
                else
                {
                    existingItem.Quantity = quantity;
                    await _cartRepo.UpdateCartItemAsync(existingItem);
                }
            }
        }

        public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId);
            if (existingItem != null)
            {
                await _cartRepo.RemoveFromCartAsync(existingItem);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            await _cartRepo.ClearCartAsync(userId);
        }
    }
}
