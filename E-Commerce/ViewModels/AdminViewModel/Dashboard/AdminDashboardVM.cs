using System.Security.Permissions;

namespace E_Commerce.ViewModels.AdminViewModel.Dashboard
{
    public class AdminDashboardVM
    {
        public decimal TotalRevenue { get; set; }
        public int Totalorders { get; set; }
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int ActiveOrders { get; set; }
        public int PendingOrders { get; set; }
        public int OrdersLastweek { get; set; }
        public ICollection<OrderDashboardVM> Orders { get; set; }
        public ICollection<ProductDashboardVM> Products { get; set; }

    }


}
