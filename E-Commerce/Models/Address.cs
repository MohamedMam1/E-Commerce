using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  

        [Required]
        public string AddressLine { get; set; }  

        // Navigation
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}