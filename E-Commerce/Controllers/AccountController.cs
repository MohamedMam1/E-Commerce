using E_Commerce.ViewModels;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM userFromReq)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser
                {
                    FullName = userFromReq.FullName?.Trim(),
                    UserName = userFromReq.Email?.Trim(),
                    PhoneNumber = userFromReq.Phone?.Trim()
                };

                var result = await _userManager.CreateAsync(appUser, userFromReq.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(userFromReq);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM userFromReq)
        {
            if (!ModelState.IsValid)
                return View(userFromReq);

            var result = await _signInManager.PasswordSignInAsync(
                userFromReq.Email?.Trim(),
                userFromReq.Password,
                userFromReq.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            ModelState.AddModelError("", "Invalid email or password.");
            return View(userFromReq);
        }

        // GET: /Account/SignOut
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}