using E_Commerce.ViewModels.Product;
using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{

    public class AdminDashboardDetailVM
    {
        public List<UserDashBoardVM> Users { get; set; }
        public List<ProductListVM> Products { get; set; }
    }
}
