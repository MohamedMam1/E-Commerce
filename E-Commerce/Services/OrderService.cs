using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
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

        public async Task<PaginatedResultVM<AdminOrderSummaryVM>> GetFilteredOrdersForAdminAsync(string? SearchTerm, string? Status, DateTime? DateFrom, DateTime? DateTo, int PageNumber, int PageSize)
        {
            OrderStatus? ParsedStatus = null;

            if (!string.IsNullOrWhiteSpace(Status) &&
                Enum.TryParse<OrderStatus>(Status, true, out var TempStatus))
            {
                ParsedStatus = TempStatus;
            }

            var Result = await _orderRepository.SearchAndFilterAsync(
                SearchTerm,
                ParsedStatus,
                DateFrom,
                DateTo,
                null,
                null,
                PageNumber,
                PageSize);

            List<AdminOrderSummaryVM> OrdersVm = Result.Orders
                .Select(O => new AdminOrderSummaryVM
                {
                    OrderId = O.Id,
                    UserName = O.User != null ? O.User.FullName : string.Empty,
                    UserEmail = O.User != null ? O.User.Email : string.Empty,
                    OrderDate = O.CreatedAt,
                    TotalAmount = O.TotalAmount,
                    ItemsCount = O.OrderItems != null ? O.OrderItems.Count : 0,
                    Status = O.Status
                })
                .ToList();

            return new PaginatedResultVM<AdminOrderSummaryVM>
            {
                Data = OrdersVm,
                TotalCount = Result.TotalCount,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)Result.TotalCount / PageSize)
            };
        }

        public async Task<bool> UpdateOrderStatusAsync(int OrderId, string NewStatus)
        {
            if (!Enum.TryParse<OrderStatus>(NewStatus, true, out var ParsedStatus))
            {
                return false;
            }

            return await _orderRepository.UpdateOrderStatusAsync(OrderId, ParsedStatus);
        }
        public async Task<AdminOrderDetailsVM?> GetOrderDetailsForAdminAsync(int orderId)
        {
            Order? order = await _orderRepository.GetOrderDetailsForAdminAsync(orderId);

            if (order == null)
            {
                return null;
            }

            AdminOrderDetailsVM result = new AdminOrderDetailsVM
            {
                OrderId = order.Id,
                OrderDate = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                UserId = order.UserId,
                UserName = order.User != null ? order.User.FullName : string.Empty,
                UserEmail = order.User != null ? order.User.Email : string.Empty,
                Items = order.OrderItems != null
                    ? order.OrderItems.Select(OI => new AdminOrderItemDetailsVM
                    {
                        ProductId = OI.ProductId,
                        ProductName = OI.Product != null ? OI.Product.Name : string.Empty,
                        ProductImage = OI.Product != null ? OI.Product.ImageUrl : string.Empty,
                        Quantity = OI.Quantity,
                        UnitPrice = OI.Price,
                        SubTotal = OI.Quantity * OI.Price
                    }).ToList()
                    : new List<AdminOrderItemDetailsVM>()
            };

            return result;
        }
    }
}
