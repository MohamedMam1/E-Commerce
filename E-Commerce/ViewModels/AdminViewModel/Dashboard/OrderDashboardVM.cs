using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminViewModel.Dashboard
{
    public class OrderDashboardVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }


}
