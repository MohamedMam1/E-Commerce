using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Account;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                FullName = model.FullName?.Trim(),
                UserName = model.Email?.Trim(),
                Email = model.Email?.Trim(),
                PhoneNumber = model.Phone?.Trim(),
                Address = model.Address?.Trim(),
                City = model.City?.Trim(),
                PostalCode = model.PostalCode?.Trim(),
                Country = model.Country?.Trim()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SendVerificationEmailAsync(user);
                return RedirectToAction("EmailSent");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return View("VerifyEmailSuccess");

            return View("Error");
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
        
            var user = await _userManager.FindByEmailAsync(model.Email.Trim());

            if (user != null)
            {
                if (user.Status == UserStatus.Banned)
                {
                    ModelState.AddModelError("", "Your account has been banned. Please contact the administrator for more details."); 
                    return View(model);
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    TempData["UnverifiedEmail"] = user.Email;
                    ModelState.AddModelError("", "Email verification is required. Please verify your email before login.");
                    return View(model);
                }
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email.Trim(),
                model.Password,
                model.RememberMe,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            // Check if user is admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            // Regular user goes to user dashboard
            return RedirectToAction("Index", "UserDashboard");
        }


        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> IsEmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(false);

            var exists = await _userService.IsEmailExists(email);
            return Json(!exists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerificationEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ResendError"] = "Email is required.";
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByEmailAsync(email.Trim());

            if (user == null)
            {
                TempData["ResendError"] = "No account found with that email.";
                return RedirectToAction("Login");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToAction("Login");
            }

            await SendVerificationEmailAsync(user);

            TempData["Message"] = "Verification email sent successfully. Please check your inbox.";
            return RedirectToAction("EmailSent");
        }

        [HttpGet]
        public IActionResult EmailSent()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("EmailSent");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Email must be verified first. Please verify your email before resetting your password.");
                TempData["UnverifiedEmail"] = user.Email;
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action("ResetPassword", "Account",
                new { email = user.Email, token = encodedToken },
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset Password",
                $"<h3>Reset your password:</h3>" +
                $"<a href='{link}'>Reset Password</a>"
            );

            return RedirectToAction("EmailSent");
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
                return BadRequest();

            var model = new ResetPasswordVM
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("Login");

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(model.Token));

            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                model.NewPassword
            );

            if (result.Succeeded)
                return RedirectToAction("Login");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }



        private async Task SendVerificationEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action("VerifyEmail", "Account",
                new { userId = user.Id, token = encodedToken },
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"<h3>Click below to confirm your email:</h3>" +
                $"<a href='{link}'>Confirm Email</a>"
            );
        }
    }
}
