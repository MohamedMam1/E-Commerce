using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Interfaces;
using FinalProject.Models;

namespace E_Commerce.Interfaces
{
    public interface IOrderService : IService<Order>
    {
        Task<List<UserOrderSummaryVM>> GetRecentOrdersByUserIdAsync(string UserId);

        Task<List<UserOrderSummaryVM>> GetAllOrdersByUserIdAsync(string UserId);

        Task<UserOrderDetailsVM?> GetOrderDetailsByUserIdAsync(string UserId, int OrderId);
        Task<PaginatedResultVM<AdminOrderSummaryVM>> GetFilteredOrdersForAdminAsync(string? SearchTerm,string? Status,DateTime? DateFrom,
          DateTime? DateTo, int PageNumber,int PageSize);
        Task<bool> UpdateOrderStatusAsync(int OrderId, string NewStatus);
        Task<AdminOrderDetailsVM?> GetOrderDetailsForAdminAsync(int orderId);
    }
}

