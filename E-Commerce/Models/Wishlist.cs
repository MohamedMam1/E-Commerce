using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Wishlist
    {
        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
