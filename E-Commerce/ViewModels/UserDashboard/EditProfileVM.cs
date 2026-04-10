using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.UserDashboard
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        public List<UserAddressVM> Addresses { get; set; } = new List<UserAddressVM>();

        [Display(Name = "New Address")]
        public string? NewAddressLine { get; set; }
    }
}
