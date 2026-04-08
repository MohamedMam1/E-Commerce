using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class AdminOrderSummaryVM
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemsCount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
