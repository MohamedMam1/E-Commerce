using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class UserDashBoardVM
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
