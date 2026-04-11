using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using E_Commerce.ViewModels;
using E_Commerce.Interfaces;
using FinalProject.Models;
using Stripe;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(
            IConfiguration config,
            IPaymentService paymentService,
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _paymentService = paymentService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _cartService.GetUserCartAsync(user.Id);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutVM
            {
                StripePublishableKey = _config["Stripe:PublishableKey"],
                CartItems = cart.Items.Select(i => new CartItemVM
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = string.Empty
            };

            model.TotalAmount = model.CartItems.Sum(x => x.Subtotal);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(
            string stripeToken,
            decimal totalAmount,
            string fullName,
            string email,
            string phone,
            string address)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _cartService.GetUserCartAsync(user.Id);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)(totalAmount * 100),
                Currency = "usd",
                Description = $"E-Commerce Order - {fullName}",
                Source = stripeToken
            };

            var chargeService = new ChargeService();

            try
            {
                Charge charge = chargeService.Create(chargeOptions);

                if (charge.Status == "succeeded")
                {
                    var items = cart.Items
                        .Select(i => (i.ProductId, i.Quantity, i.Price))
                        .ToList();

                    await _paymentService.SaveOrderAsync(user.Id, items);

                    return View("SuccessPayment", new PaymentSuccessVM
                    {
                        ChargeId = charge.Id,
                        TotalAmount = totalAmount
                    });
                }
                else
                {
                    return View("FailedPayment", new PaymentFailedVM
                    {
                        ErrorMessage = "Payment failed. Please try again."
                    });
                }
            }
            catch (StripeException ex)
            {
                return View("FailedPayment", new PaymentFailedVM
                {
                    ErrorMessage = ex.StripeError.Message
                });
            }
        }
    }
}