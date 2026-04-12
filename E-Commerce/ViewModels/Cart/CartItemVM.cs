using FinalProject.Models;

namespace E_Commerce.ViewModels.Cart
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public ProductSize Size { get; set; }
        public ProductColor Color { get; set; }
        public int MaxQuantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
