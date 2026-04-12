namespace E_Commerce.ViewModels.UserDashboard
{
    public class UserDashboardVM
    {
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public List<UserOrderSummaryVM> RecentOrders { get; set; } = new List<UserOrderSummaryVM>();
    }
}

