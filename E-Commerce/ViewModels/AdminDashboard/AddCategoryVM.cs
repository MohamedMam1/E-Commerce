using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class AddCategoryVM
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        [Remote("CheckCategoryName", "AdminDashboard", ErrorMessage = "A category with this name already exists.")]
        public string CategoryName { get; set; }
    }
}