using E_Commerce.Interfaces;
using E_Commerce.Services;
using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            IAdminDashboardService adminDashboardService,
            IProductService productService,
            ICategoryService categoryService,
             IOrderService orderService,
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _adminDashboardService = adminDashboardService;
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var dashboardVM = await _adminDashboardService.GetDashBoardDetails();
            return View(dashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpGet]
        public IActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsTable(string searchTerm, int? categoryId, bool? isAvailable, int page = 1)
        {
            var result = await _productService.GetFilteredProductsAsync(searchTerm, categoryId, isAvailable, page, 10);
            return PartialView("_ProductTable", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesTable(string searchTerm, int page = 1)
        {
            var result = await _categoryService.GetFilteredCategoriesAsync(searchTerm, page, 10);
            return PartialView("_CategoryTable", result);
        }

        
        [HttpGet]
        public IActionResult Orders()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OrdersTable(string? searchTerm, string? status, DateTime? dateFrom, DateTime? dateTo, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _orderService.GetFilteredOrdersForAdminAsync(searchTerm, status, dateFrom, dateTo, pageNumber, pageSize);
            return PartialView("_OrdersTable", result);
        }

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

   
        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new { success = false, roles = new List<string>() });
            }

            ApplicationUser? User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return Json(new { success = false, roles = new List<string>() });
            }

            IList<string> Roles = await _userManager.GetRolesAsync(User);

            return Json(new { success = true, roles = Roles.ToList() });
        }

        [HttpGet]
        public async Task<IActionResult> UsersTable(string? searchTerm, string? status, string? role, int pageNumber = 1, int pageSize = 10)
        {
            var model = await _userService.GetUsersWithRolesAsync(searchTerm, status, role, pageNumber, pageSize);
            return PartialView("_UsersTable", model);
        }

   
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

            // Check if user already has this role
            bool UserHasRole = await _userManager.IsInRoleAsync(User, RoleName);
            if (UserHasRole)
            {
                return Json(new { success = false, message = "User already has this role." });
            }

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

            // Add the role without removing existing ones
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

        [HttpPost]
        public async Task<IActionResult> RemoveUserRole(string id, [FromBody] UpdateUserRoleVM model)
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

            // Prevent user from removing their own admin role
            var currentUserId = this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (currentUserId == id && model.RoleName.Trim() == "Admin")
            {
                return Json(new { success = false, message = "You cannot remove your own admin role." });
            }

            string RoleName = model.RoleName.Trim();

            IdentityResult RemoveRoleResult = await _userManager.RemoveFromRoleAsync(User, RoleName);

            if (!RemoveRoleResult.Succeeded)
            {
                return Json(new
                {
                    success = false,
                    message = RemoveRoleResult.Errors.FirstOrDefault()?.Description ?? "Failed to remove role."
                });
            }

            return Json(new { success = true });
        }

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

            // Prevent user from banning themselves
            var currentUserId = this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == id && model.Status.Trim() == "Banned")
            {
                return Json(new { success = false, message = "You cannot ban yourself." });
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

        public IActionResult AccountSettings()
        {
            return RedirectToAction(nameof(EditAdminAccount));
        }

        [HttpGet]
        public async Task<IActionResult> EditAdminAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new EditAdminAccountVM
            {
                FullName = user.FullName,
                Email = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                Country = user.Country
            };

            return View("AccountSettings", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdminAccount(EditAdminAccountVM model)
        {
            if (!ModelState.IsValid)
                return View("AccountSettings", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Update profile information
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.PostalCode = model.PostalCode;
            user.Country = model.Country;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("AccountSettings", model);
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is required to change your password.");
                    return View("AccountSettings", model);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View("AccountSettings", model);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
