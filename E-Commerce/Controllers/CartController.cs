using E_Commerce.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartVm = await _cartService.GetUserCartAsync(userId);
            return View(cartVm);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, int size = 0, int color = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please log in to add items to your cart." });

            if (!TryParseVariant(size, color, out var productSize, out var productColor))
                return Json(new { success = false, message = "Please select a valid size and color." });

            var (success, message) = await _cartService.AddItemToCartAsync(
                userId, productId, quantity, productSize, productColor);

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity, int size, int color)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please log in." });

            if (!TryParseVariant(size, color, out var productSize, out var productColor))
                return Json(new { success = false, message = "Invalid cart line." });

            var (success, message) = await _cartService.UpdateCartItemQuantityAsync(
                userId, productId, quantity, productSize, productColor);

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId, int size, int color)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!TryParseVariant(size, color, out var productSize, out var productColor))
                return RedirectToAction(nameof(Index));

            await _cartService.RemoveItemFromCartAsync(userId, productId, productSize, productColor);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.ClearCartAsync(userId);

            return RedirectToAction(nameof(Index));
        }

        private static bool TryParseVariant(int size, int color, out ProductSize productSize, out ProductColor productColor)
        {
            productSize = (ProductSize)size;
            productColor = (ProductColor)color;

            if (!Enum.IsDefined(typeof(ProductSize), size) || !Enum.IsDefined(typeof(ProductColor), color))
                return false;

            return productSize != ProductSize.Unspecified && productColor != ProductColor.Unspecified;
        }
    }
}
