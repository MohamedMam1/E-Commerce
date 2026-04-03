namespace E_Commerce.ViewModels.UserDashboard
{
    public class UserOrderDetailsVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public List<UserOrderItemDetailsVM> Items { get; set; } = new List<UserOrderItemDetailsVM>();
    }
}

