namespace E_Commerce.ViewModels.UserDashboard
{
    public class UserDashboardVM
    {
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserAddressVM> Addresses { get; set; } = new List<UserAddressVM>();
        public List<UserOrderSummaryVM> RecentOrders { get; set; } = new List<UserOrderSummaryVM>();
    }
}
