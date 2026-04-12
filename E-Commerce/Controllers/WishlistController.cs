using E_Commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
   [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistVm = await _wishlistService.GetUserWishlistAsync(userId);
            return View(wishlistVm);
        }

       
        [HttpPost]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            if (productId <= 0)
                return Json(new { success = false, message = "Invalid product." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please log in to use the wishlist." });

            await _wishlistService.ToggleWishlistItemAsync(userId, productId);

            return Json(new { success = true, message = "Wishlist updated!" });
        }

        
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _wishlistService.RemoveItemFromWishlistAsync(userId, productId);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
