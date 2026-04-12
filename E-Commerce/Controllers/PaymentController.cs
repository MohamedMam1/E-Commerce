using Microsoft.AspNetCore.Mvc;
using E_Commerce.ViewModels;
using E_Commerce.ViewModels.Cart;
using E_Commerce.Interfaces;
using Stripe;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;

        public PaymentController(IConfiguration config, IPaymentService paymentService, ICartService cartService)
        {
            _config = config;
            _paymentService = paymentService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetUserCartAsync(userId);

            var model = new CheckoutVM
            {
                StripePublishableKey = _config["Stripe:PublishableKey"],
                CartItems = cart.Items.Select(i => new CartItemVM
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Size = i.Size,
                    Color = i.Color,
                    MaxQuantity = i.MaxQuantity
                }).ToList()
            };

            model.TotalAmount = model.CartItems.Sum(x => x.Subtotal);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string stripeToken, decimal totalAmount)
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)(totalAmount * 100),
                Currency = "usd",
                Description = "E-Commerce Order",
                Source = stripeToken
            };

            var chargeService = new ChargeService();

            try
            {
                Charge charge = chargeService.Create(chargeOptions);

                if (charge.Status == "succeeded")
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var cart = await _cartService.GetUserCartAsync(userId);

                    var items = cart.Items
                            .Select(i => (ProductVariantId: i.ProductVariantId, i.Quantity, i.Price)) // ✅ Works now
                            .ToList(); ;

                    await _paymentService.SaveOrderAsync(userId, items);

                    // Clear the cart after successful payment
                    await _cartService.ClearCartAsync(userId);

                    var successModel = new PaymentSuccessVM
                    {
                        ChargeId = charge.Id,
                        TotalAmount = totalAmount
                    };

                    return View("SuccessPayment", successModel);
                }
                else
                {
                    var failedModel = new PaymentFailedVM
                    {
                        ErrorMessage = "Payment failed. Please try again."
                    };

                    return View("FailedPayment", failedModel);
                }
            }
            catch (StripeException ex)
            {
                var failedModel = new PaymentFailedVM
                {
                    ErrorMessage = ex.StripeError.Message
                };

                return View("FailedPayment", failedModel);
            }
        }
    }
}
