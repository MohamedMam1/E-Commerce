using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.UserDashboard
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        [StringLength(100)]
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
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

