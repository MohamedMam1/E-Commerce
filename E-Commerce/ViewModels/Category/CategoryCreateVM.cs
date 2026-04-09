using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
