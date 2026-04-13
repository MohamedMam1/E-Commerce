using E_Commerce.Interfaces;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "User")]
    public class UserDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        //Counstructor
        public UserDashboardController(UserManager<ApplicationUser> userManager , IOrderService orderService , IUserService userService)
        {
            this._userManager = userManager;
            this._orderService = orderService;
            this._userService = userService;
        }

        //Actions
        #region Index Action
        public async Task<IActionResult> Index()
        {
            ApplicationUser? CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return Challenge();
            }

            List<UserOrderSummaryVM> RecentOrders = await _orderService.GetRecentOrdersByUserIdAsync(CurrentUser.Id);

            UserDashboardVM userVM = new UserDashboardVM
            {
                FullName = CurrentUser.FullName,
                Email = CurrentUser.UserName,
                PhoneNumber = CurrentUser.PhoneNumber,
                Address = CurrentUser.Address,
                City = CurrentUser.City,
                PostalCode = CurrentUser.PostalCode,
                Country = CurrentUser.Country,
                RecentOrders = RecentOrders
            };

            return View("Index", userVM);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser? currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            EditProfileVM editedUserVM = new EditProfileVM
            {
                FullName = currentUser.FullName,
                Email = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                Address = currentUser.Address,
                City = currentUser.City,
                PostalCode = currentUser.PostalCode,
                Country = currentUser.Country,
            };

            return View(editedUserVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.PostalCode = model.PostalCode;
            user.Country = model.Country;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is required.");
                    return View(model);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors) ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }
            return RedirectToAction("Index"); 
        }


        public async Task<IActionResult> Orders()
        {
            ApplicationUser? CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return Challenge();
            }

            List<UserOrderSummaryVM> UserOrders = await _orderService.GetAllOrdersByUserIdAsync(CurrentUser.Id);

            return View("Orders",UserOrders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            ApplicationUser? CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return Challenge();
            }

            UserOrderDetailsVM? OrderDetailsVm = await _orderService.GetOrderDetailsByUserIdAsync(CurrentUser.Id, id);

            if (OrderDetailsVm == null)
            {
                return NotFound();
            }

            return View("OrderDetails",OrderDetailsVm);
        }
    }
}
