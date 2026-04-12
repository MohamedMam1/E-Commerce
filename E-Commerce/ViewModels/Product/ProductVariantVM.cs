using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Product
{
    public class ProductVariantVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Size is required")]
        [StringLength(50)]
        public string Size { get; set; }

        [Required(ErrorMessage = "Color is required")]
        [StringLength(50)]
        public string Color { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater")]
        public int Stock { get; set; }
    }
}
