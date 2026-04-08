using FinalProject.Models;

namespace E_Commerce.ViewModels.AdminDashboard
{
    public class OrderFilterVM
    {
        public string SearchTerm { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
