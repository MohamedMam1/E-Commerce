using E_Commerce.Interfaces;
using E_Commerce.Services;
using E_Commerce.ViewModels.AdminDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace E_Commerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AdminController(
            IAdminDashboardService adminDashboardService,
            IProductService productService,
            IOrderService orderService,
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _adminDashboardService = adminDashboardService;
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
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

        
        #region Order
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Orders()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> OrdersTable(string? searchTerm, string? status, DateTime? dateFrom, DateTime? dateTo, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _orderService.GetFilteredOrdersForAdminAsync(searchTerm, status, dateFrom, dateTo, pageNumber, pageSize);
            return PartialView("_OrdersTable", result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusVM model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Status))
            {
                return Json(new { success = false, message = "Invalid status data." });
            }

            bool isUpdated = await _orderService.UpdateOrderStatusAsync(id, model.Status);

            if (!isUpdated)
            {
                return Json(new { success = false, message = "Failed to update order status." });
            }

            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            AdminOrderDetailsVM? order = await _orderService.GetOrderDetailsForAdminAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        #endregion

        #region User
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var userPagination = await _userService.GetUsersWithRolesAsync(null, null, null, 1, 10);
            var roles = _roleManager.Roles
                .Select(R => R.Name)
                .Where(R => !string.IsNullOrWhiteSpace(R))
                .ToList();

            var vm = new AdminDashboardDetailVM
            {
                Users = userPagination.Users,
                UserPagination = userPagination,
                Roles = roles!
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UsersTable(string? searchTerm, string? status, string? role, int pageNumber = 1, int pageSize = 10)
        {
            var model = await _userService.GetUsersWithRolesAsync(searchTerm, status, role, pageNumber, pageSize);
            return PartialView("_UsersTable", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UsersRole(string id, [FromBody] UpdateUserRoleVM model)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new { success = false, message = "Invalid user id." });
            }

            if (model == null || string.IsNullOrWhiteSpace(model.RoleName))
            {
                return Json(new { success = false, message = "Role name is required." });
            }

            ApplicationUser? User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            string RoleName = model.RoleName.Trim();

            bool RoleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!RoleExists)
            {
                IdentityResult CreateRoleResult = await _roleManager.CreateAsync(new IdentityRole(RoleName));

                if (!CreateRoleResult.Succeeded)
                {
                    return Json(new
                    {
                        success = false,
                        message = CreateRoleResult.Errors.FirstOrDefault()?.Description ?? "Failed to create role."
                    });
                }
            }

            IList<string> CurrentRoles = await _userManager.GetRolesAsync(User);

            if (CurrentRoles.Any())
            {
                IdentityResult RemoveRolesResult = await _userManager.RemoveFromRolesAsync(User, CurrentRoles);

                if (!RemoveRolesResult.Succeeded)
                {
                    return Json(new
                    {
                        success = false,
                        message = RemoveRolesResult.Errors.FirstOrDefault()?.Description ?? "Failed to remove current roles."
                    });
                }
            }

            IdentityResult AddRoleResult = await _userManager.AddToRoleAsync(User, RoleName);

            if (!AddRoleResult.Succeeded)
            {
                return Json(new
                {
                    success = false,
                    message = AddRoleResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign role."
                });
            }

            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string id, [FromBody] UpdateUserStatusVM model)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new { success = false, message = "Invalid user id." });
            }

            if (model == null || string.IsNullOrWhiteSpace(model.Status))
            {
                return Json(new { success = false, message = "Invalid status data." });
            }

            ApplicationUser? User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            string NewStatus = model.Status.Trim();

            if (NewStatus == "Banned")
            {
                User.LockoutEnabled = true;
                User.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                User.Status = UserStatus.Banned;
            }
            else if (NewStatus == "Active")
            {
                User.LockoutEnd = null;
                User.Status = UserStatus.Active;
            }
            else
            {
                return Json(new { success = false, message = "Invalid status value." });
            }

            IdentityResult UpdateResult = await _userManager.UpdateAsync(User);

            if (!UpdateResult.Succeeded)
            {
                return Json(new
                {
                    success = false,
                    message = UpdateResult.Errors.FirstOrDefault()?.Description ?? "Failed to update user status."
                });
            }

            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new { success = false, message = "Invalid user id." });
            }

            ApplicationUser? User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            IdentityResult DeleteResult = await _userManager.DeleteAsync(User);

            if (!DeleteResult.Succeeded)
            {
                return Json(new
                {
                    success = false,
                    message = DeleteResult.Errors.FirstOrDefault()?.Description ?? "Failed to delete user."
                });
            }

            return Json(new { success = true });
        } 
        #endregion

    }
}
