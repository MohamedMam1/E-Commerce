namespace E_Commerce.ViewModels
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string StripePublishableKey { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
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