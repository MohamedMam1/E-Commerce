using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.UserDashboard
{
    public class UserAddressVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string AddressLine { get; set; }

    }
}
