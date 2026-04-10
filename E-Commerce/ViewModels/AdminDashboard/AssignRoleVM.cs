using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class AssignRoleVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Remote("CheckRoleName", "AdminDashboard", AdditionalFields = "UserId", ErrorMessage = "This role is already assigned to the user or does not exist.")]
        public string RoleName { get; set; }

        public string UserName { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public List<string> CurrentRoles { get; set; } = new List<string>();
    }
}