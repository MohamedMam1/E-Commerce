using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
