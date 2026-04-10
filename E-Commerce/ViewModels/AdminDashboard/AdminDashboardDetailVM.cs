using E_Commerce.ViewModels.Product;
using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class AdminDashboardDetailVM
    {
        public DashboardStatsVM Stats { get; set; }
        public List<UserDashBoardVM> Users { get; set; } = new List<UserDashBoardVM>();
        public List<ProductListVM> Products { get; set; } = new List<ProductListVM>();
        public List<AdminOrderSummaryVM> Orders { get; set; } = new List<AdminOrderSummaryVM>();
        public List<AddCategoryVM> Categories { get; set; } = new List<AddCategoryVM>();
        public List<string> Roles { get; set; } = new List<string>();
        public UserPaginationVM UserPagination { get; set; } = new UserPaginationVM();
    }
}
