using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get ; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
