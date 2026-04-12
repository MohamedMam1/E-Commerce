using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Cart;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly ITiContext _context;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo, ITiContext context)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _context = context;
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
                    CartItemId = c.Id,
                    ProductVariantId = c.ProductVariantId, 
                    ProductId = c.ProductVariant.ProductId,
                    ProductName = c.ProductVariant.Product?.Name ?? "Unknown Product",
                    Price = c.ProductVariant.Product?.Price ?? 0,
                    ImageUrl = c.ProductVariant.Product?.ImageUrl,
                    Quantity = c.Quantity,
                    Size = c.ProductVariant.Size,
                    Color = c.ProductVariant.Color,
                    MaxQuantity = c.ProductVariant.Stock
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
            string userId, int productVariantId, int quantity)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, "You must be logged in to add items to your cart.");

            if (quantity <= 0)
                return (false, "Quantity must be at least 1.");

            var variant = await _context.ProductVariants
                .Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.Id == productVariantId);
            
            if (variant == null)
                return (false, "Product variant not found.");

            if (variant.Stock <= 0)
                return (false, "This product is out of stock.");

            var existingItem = await _cartRepo.GetCartItemAsync(userId, productVariantId);
            var inCart = existingItem?.Quantity ?? 0;
            var totalRequested = inCart + quantity;

            if (totalRequested > variant.Stock)
            {
                if (inCart > 0)
                    return (false, $"Only {variant.Stock} in stock. You already have {inCart} in your cart.");
                return (false, $"Only {variant.Stock} item(s) available in stock.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity = totalRequested;
                await _cartRepo.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    ProductVariantId = productVariantId,
                    Quantity = quantity
                };
                await _cartRepo.AddToCartAsync(newItem);
            }

            return (true, "Item added to cart!");
        }

        public async Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(
            string userId, int cartItemId, int quantity)
        {
            if (string.IsNullOrEmpty(userId))
                return (false, "You must be logged in.");

            var existingItem = await _context.CartItems
                .Include(c => c.ProductVariant)
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
            
            if (existingItem == null)
                return (false, "Cart item not found.");

            var variant = existingItem.ProductVariant;
            var maxQty = variant?.Stock ?? 0;

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

        public async Task RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
            if (item != null)
            {
                await _cartRepo.RemoveFromCartAsync(item);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            await _cartRepo.ClearCartAsync(userId);
        }
    }
}


