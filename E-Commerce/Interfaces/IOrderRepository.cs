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
    }
}
