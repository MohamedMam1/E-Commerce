using E_Commerce.Interfaces;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize]
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
            List<UserAddressVM> UserAddresses = await _userService.GetUserAddressesAsync(CurrentUser.Id);
            List<UserOrderSummaryVM> RecentOrders = await _orderService.GetRecentOrdersByUserIdAsync(CurrentUser.Id);

            UserDashboardVM userVM = new UserDashboardVM
            {
                FullName = CurrentUser.FullName,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,
                Addresses = UserAddresses,
                RecentOrders = RecentOrders
            };

            return View("Index", userVM);
        }
        #endregion

        #region Edit UserData
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser? CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return Challenge();
            }
            List<UserAddressVM> UserAddresses = await _userService.GetUserAddressesAsync(CurrentUser.Id);
            EditProfileVM editedUserVM = new EditProfileVM
            {
                FullName = CurrentUser.FullName,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,
                Addresses = UserAddresses
            };

            return View(editedUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileVM editedUserFromRq)
        {
            if (!ModelState.IsValid)
            {
                return View("EditProfile",editedUserFromRq);
            }

            ApplicationUser? CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return Challenge();
            }

            CurrentUser.FullName = editedUserFromRq.FullName;
            CurrentUser.PhoneNumber = editedUserFromRq.PhoneNumber;

            IdentityResult editEmailResult = await _userManager.SetEmailAsync(CurrentUser, editedUserFromRq.Email);

            if (!editEmailResult.Succeeded)
            {
                foreach (IdentityError Error in editEmailResult.Errors)
                {
                    ModelState.AddModelError("", Error.Description);
                }

                return View("EditProfile",editedUserFromRq);
            }

            IdentityResult updateResult = await _userManager.UpdateAsync(CurrentUser);

            if (!updateResult.Succeeded)
            {
                foreach (IdentityError Error in updateResult.Errors)
                {
                    ModelState.AddModelError("", Error.Description);
                }

                return View("EditProfile",editedUserFromRq);
            }

            await _userService.UpdateUserAddressesAsync(CurrentUser.Id, editedUserFromRq.Addresses, editedUserFromRq.NewAddressLine);

            return RedirectToAction("Index");
        }
        #endregion
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
