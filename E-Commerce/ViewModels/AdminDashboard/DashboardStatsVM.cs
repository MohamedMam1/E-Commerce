namespace E_Commerce.ViewModels.AdminDashboard
{
    public class DashboardStatsVM
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int PendingOrders { get; set; }
        public int ActiveProducts { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int RecentOrdersLastWeek { get; set; }
    }
}
