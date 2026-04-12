using E_Commerce.ViewModels.Cart;

namespace E_Commerce.ViewModels
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string StripePublishableKey { get; set; }
    }
    public class PaymentSuccessVM
    {
        public string ChargeId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PaymentFailedVM
    {
        public string ErrorMessage { get; set; }
    }
}