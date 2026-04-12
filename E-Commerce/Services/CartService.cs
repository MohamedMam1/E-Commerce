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
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<CartVM> GetUserCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new CartVM { Items = Array.Empty<CartItemVM>() };
            }

            var cartItems = await _cartRepo.GetCartItemsAsync(userId);

            var vm = new CartVM
            {
                Items = cartItems.Select(c => new CartItemVM
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product?.Name ?? "Unknown Product",
                    Price = c.Product?.Price ?? 0,
                    ImageUrl = c.Product?.ImageUrl,
                    Quantity = c.Quantity,
                    Size = c.Size,
                    Color = c.Color,
                    MaxQuantity = c.Product?.Quantity ?? 0
                }).ToList()
            };

            return vm;
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return 0;

            var cartItems = await _cartRepo.GetCartItemsAsync(userId);
            return cartItems.Sum(c => c.Quantity);
        }

        public async Task<(bool Success, string Message)> AddItemToCartAsync(
            string userId, int productId, int quantity, ProductSize size, ProductColor color)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, "You must be logged in to add items to your cart.");

            if (quantity <= 0)
                return (false, "Quantity must be at least 1.");

            if (size == ProductSize.Unspecified)
                return (false, "Please select a size.");

            if (color == ProductColor.Unspecified)
                return (false, "Please select a color.");

            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return (false, "Product not found.");

            if (!product.IsAvailable)
                return (false, "This product is out of stock.");

            if (product.Size != size || product.Color != color)
                return (false, "Selected size and color must match this product.");

            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId, size, color);
            var inCart = existingItem?.Quantity ?? 0;
            var totalRequested = inCart + quantity;

            if (totalRequested > product.Quantity)
            {
                if (inCart > 0)
                    return (false, $"Only {product.Quantity} in stock. You already have {inCart} in your cart.");
                return (false, $"Only {product.Quantity} item(s) available in stock.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity = totalRequested;
                await _cartRepo.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Size = size,
                    Color = color,
                    Quantity = quantity
                };
                await _cartRepo.AddToCartAsync(newItem);
            }

            return (true, "Item added to cart!");
        }

        public async Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(
            string userId, int productId, int quantity, ProductSize size, ProductColor color)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, "You must be logged in.");

            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId, size, color);
            if (existingItem == null)
                return (false, "Cart item not found.");

            var product = existingItem.Product ?? await _productRepo.GetByIdAsync(productId);
            var maxQty = product?.Quantity ?? 0;

            if (quantity > maxQty)
                return (false, $"Quantity cannot exceed {maxQty} in stock.");

            if (quantity <= 0)
            {
                await _cartRepo.RemoveFromCartAsync(existingItem);
                return (true, "Item removed.");
            }

            existingItem.Quantity = quantity;
            await _cartRepo.UpdateCartItemAsync(existingItem);
            return (true, "Quantity updated.");
        }

        public async Task RemoveItemFromCartAsync(string userId, int productId, ProductSize size, ProductColor color)
        {
            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId, size, color);
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
