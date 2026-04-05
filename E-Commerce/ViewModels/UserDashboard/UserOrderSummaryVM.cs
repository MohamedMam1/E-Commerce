namespace E_Commerce.ViewModels.UserDashboard
{
    public class UserOrderSummaryVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ItemsCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
