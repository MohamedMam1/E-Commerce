using E_Commerce.Interfaces;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Models;
using System.Linq.Expressions;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository OrderRepository)
        {
            _orderRepository = OrderRepository;
        }

        public async Task<List<UserOrderSummaryVM>> GetRecentOrdersByUserIdAsync(string UserId)
        {
            List<Order> Orders = await _orderRepository.GetRecentOrdersByUserIdAsync(UserId, 5);

            List<UserOrderSummaryVM> Result = Orders
                .Select(O => new UserOrderSummaryVM
                {
                    OrderId = O.Id,
                    OrderDate = O.CreatedAt,
                    ItemsCount = O.OrderItems != null ? O.OrderItems.Count : 0,
                    TotalPrice = O.OrderItems != null
                        ? O.OrderItems.Sum(OI => OI.Quantity * OI.Price)
                        : 0
                })
                .ToList();

            return Result;
        }

        public async Task<List<UserOrderSummaryVM>> GetAllOrdersByUserIdAsync(string UserId)
        {
            List<Order> Orders = await _orderRepository.GetAllOrdersByUserIdAsync(UserId);

            List<UserOrderSummaryVM> Result = Orders
                .Select(O => new UserOrderSummaryVM
                {
                    OrderId = O.Id,
                    OrderDate = O.CreatedAt,
                    ItemsCount = O.OrderItems != null ? O.OrderItems.Count : 0,
                    TotalPrice = O.OrderItems != null
                        ? O.OrderItems.Sum(OI => OI.Quantity * OI.Price)
                        : 0
                })
                .ToList();

            return Result;
        }

        public async Task<UserOrderDetailsVM?> GetOrderDetailsByUserIdAsync(string UserId, int OrderId)
        {
            Order? Order = await _orderRepository.GetOrderDetailsByUserIdAsync(UserId, OrderId);

            if (Order == null)
            {
                return null;
            }

            UserOrderDetailsVM Result = new UserOrderDetailsVM
            {
                OrderId = Order.Id,
                OrderDate = Order.CreatedAt,
                TotalPrice = Order.OrderItems != null ? Order.OrderItems.Sum(OI => OI.Quantity * OI.Price): 0,
                Items = Order.OrderItems != null
                    ? Order.OrderItems.Select(OI => new UserOrderItemDetailsVM
                    {
                        ProductId = OI.ProductId,
                        ProductName = OI.Product != null ? OI.Product.Name : string.Empty,
                        ProductImage = OI.Product != null ? OI.Product.ImageUrl : string.Empty,
                        Quantity = OI.Quantity,
                        UnitPrice = OI.Price,
                        SubTotal = OI.Quantity * OI.Price
                    }).ToList()
                    : new List<UserOrderItemDetailsVM>()
            };

            return Result;
        }

        public Task<Order> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Order entity)
        {
            throw new NotImplementedException();
        }
        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
