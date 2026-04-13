using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class EditAdminAccountVM
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [StringLength(20, ErrorMessage = "Postal Code cannot exceed 20 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
