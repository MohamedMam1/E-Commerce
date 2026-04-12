using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        [Required]
        public int ProductVariantId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; }
    }
}
