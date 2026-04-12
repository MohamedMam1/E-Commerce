using FinalProject.Models;

namespace E_Commerce.ViewModels.Wishlist
{
    public class WishlistItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public ProductSize Size { get; set; }
        public ProductColor Color { get; set; }
    }
}
