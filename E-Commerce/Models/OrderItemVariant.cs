using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class OrderItemVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        [Required]
        public int ProductVariantId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string Color { get; set; }

        // Navigation
        [ForeignKey("OrderItemId")]
        public OrderItem OrderItem { get; set; }

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; }
    }
}
