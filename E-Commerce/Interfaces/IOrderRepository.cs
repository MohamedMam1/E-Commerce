using FinalProject.Interfaces;
using FinalProject.Models;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface IOrderRepository :IRepository<Order>
    {
        Task<List<Order>> GetRecentOrdersByUserIdAsync(string UserId, int Count);

        Task<List<Order>> GetAllOrdersByUserIdAsync(string UserId);

        Task<Order?> GetOrderDetailsByUserIdAsync(string UserId, int OrderId);

        Task<(List<Order> Orders, int TotalCount)> SearchAndFilterAsync(
            string searchTerm,
            OrderStatus? status,
            DateTime? dateFrom,
            DateTime? dateTo,
            decimal? minAmount,
            decimal? maxAmount,
            int pageNumber = 1,
            int pageSize = 10);

        Task<bool> UpdateOrderStatusAsync(int OrderId, OrderStatus NewStatus);
        Task<Order?> GetOrderDetailsForAdminAsync(int orderId);
    }
}
