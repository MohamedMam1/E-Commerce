using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Category
{
    public class CategoryEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Remote("IsNameUnique", "Category", ErrorMessage = "A Category with the same name already exists.")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
