// ViewModels/Product/ProductCreateVM.cs
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;
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

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Main image is required.")]
        public IFormFile MainImage { get; set; }

        public IFormFile? ExtraImage1 { get; set; }

        public IFormFile? ExtraImage2 { get; set; }

        public IFormFile? ExtraImage3 { get; set; }

        [Required(ErrorMessage = "At least one variant is required.")]
        public List<ProductVariantVM> Variants { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
