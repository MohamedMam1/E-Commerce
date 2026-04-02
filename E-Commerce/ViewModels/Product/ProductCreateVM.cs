using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
