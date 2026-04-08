using Microsoft.AspNetCore.Mvc;
using E_Commerce.ViewModels;
using E_Commerce.Interfaces;
using Stripe;

namespace E_Commerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IPaymentService _paymentService;

        public PaymentController(IConfiguration config, IPaymentService paymentService)
        {
            _config = config;
            _paymentService = paymentService;
        }

        public IActionResult Checkout()
        {
            var model = new CheckoutVM
            {
                StripePublishableKey = _config["Stripe:PublishableKey"],
                CartItems = new List<CartItemVM>
                {
                    new CartItemVM { ProductId = 1, ProductName = "Blue Sneakers", Quantity = 1, Price = 59.99m },
                    new CartItemVM { ProductId = 2, ProductName = "White T-Shirt",  Quantity = 2, Price = 19.99m },
                    new CartItemVM { ProductId = 3, ProductName = "Black Bag",      Quantity = 1, Price = 34.99m }
                }
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
                    string userId = "dummy-user-id-123";

                    var items = new List<(int ProductId, int Quantity, decimal Price)>
                    {
                        (1, 1, 59.99m),
                        (2, 2, 19.99m),
                        (3, 1, 34.99m)
                    };

                    await _paymentService.SaveOrderAsync(userId, items);

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