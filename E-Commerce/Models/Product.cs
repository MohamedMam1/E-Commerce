using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public int Quantity { get; set; }

        public bool IsAvailable => Quantity > 0;

        public ICollection<ProductImage> ExtraImages { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<ProductImage> ExtraImages { get; set; }
    }
}