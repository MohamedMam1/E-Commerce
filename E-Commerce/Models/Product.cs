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
        
        public bool IsAvailable => ProductVariants?.Any(v => v.Stock > 0) ?? false;

        public ICollection<ProductImage> ExtraImages { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; }
        public ICollection<CartItem> Carts { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}