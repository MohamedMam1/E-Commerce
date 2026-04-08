using Microsoft.AspNetCore.Identity;

namespace FinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    public enum UserStatus
    {
        Active,
        Banned,
    }
}
