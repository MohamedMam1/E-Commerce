using FinalProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Product
{
    public class ProductEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [Range(1, 4, ErrorMessage = "Please select a size.")]
        public ProductSize Size { get; set; }

        [Range(1, 4, ErrorMessage = "Please select a color.")]
        public ProductColor Color { get; set; }
        public string? ExistingMainImageUrl { get; set; }
        public List<string> ExistingExtraImageUrls { get; set; } = new();
        public IFormFile? MainImage { get; set; }
        public IFormFile? ExtraImage1 { get; set; }
        public IFormFile? ExtraImage2 { get; set; }
        public IFormFile? ExtraImage3 { get; set; }

        // Populated by controller
        public List<SelectListItem> Categories { get; set; } = new();
    }
}