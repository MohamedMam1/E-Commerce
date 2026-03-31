using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Changed to string since IdentityUser.Id is string

        [Required]
        public string AddressLine { get; set; }  // Renamed to AddressLine to avoid conflict with class name

        // Navigation
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}