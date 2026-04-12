using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string Size { get; set; }

        [Required]
        [StringLength(50)]
        public string Color { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public int Stock { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItemVariant> OrderItemVariants { get; set; }
    }
}
