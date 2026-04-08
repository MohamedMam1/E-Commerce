using E_Commerce.Interfaces;
using E_Commerce.Services;
using E_Commerce.ViewModels.AdminDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace E_Commerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IProductService _productService;

        public AdminController(
            IAdminDashboardService adminDashboardService,
            IProductService productService)
        {
            _adminDashboardService = adminDashboardService;
            _productService = productService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var dashboardVM = await _adminDashboardService.GetDashBoardDetails();
            return View(dashboardVM);
        }

        public async Task<IActionResult> Products()
        {
            var categories = await _adminDashboardService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTable(string searchTerm, int? categoryId, bool? isAvailable, int page = 1)
        {
            var result = await _productService.GetFilteredProductsAsync(searchTerm, categoryId, isAvailable, page, 10);
            return PartialView("_ProductTable", result);
        }
    }
}
