using Microsoft.AspNetCore.Identity;

namespace FinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        // Single address fields
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public List<CartItem> Carts { get; set; } = new List<CartItem>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    public enum UserStatus
    {
        Active,
        Banned,
    }
}
