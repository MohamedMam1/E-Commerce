// ViewModels/Product/ProductCreateVM.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.ViewModels.Product
{
    public class ProductCreateVM
    {
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

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Main image is required.")]
        public IFormFile MainImage { get; set; }

        [Required(ErrorMessage = "Extra image is required.")]
        public IFormFile? ExtraImage1 { get; set; }

        [Required(ErrorMessage = "Extra image is required.")]
        public IFormFile? ExtraImage2 { get; set; }

        [Required(ErrorMessage = "Extra image is required.")]
        public IFormFile? ExtraImage3 { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}