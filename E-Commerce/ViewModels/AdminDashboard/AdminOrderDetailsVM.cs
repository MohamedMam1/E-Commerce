using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class AdminOrderDetailsVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        public List<AdminOrderItemDetailsVM> Items { get; set; } = new();
    }
}
