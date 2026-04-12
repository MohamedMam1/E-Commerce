using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ITiContext _context;

        //Constructor
        public OrderRepository(ITiContext Context)
        {
            _context = Context;
        }

        
        //Methods
        public async Task<List<Order>> GetRecentOrdersByUserIdAsync(string UserId, int Count)
        {
            return await _context.Orders
                .Where(O => O.UserId == UserId)
                .Include(O => O.OrderItems)
                .OrderByDescending(O => O.CreatedAt)
                .Take(Count)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersByUserIdAsync(string UserId)
        {
            return await _context.Orders
                .Where(O => O.UserId == UserId)
                .Include(O => O.OrderItems)
                .OrderByDescending(O => O.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsByUserIdAsync(string UserId, int OrderId)
        {
            return await _context.Orders
                .Where(O => O.UserId == UserId && O.Id == OrderId)
                .Include(O => O.OrderItems)
                .ThenInclude(OI => OI.Product)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> GetByIdAsync(object id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(O => O.User)
                .Include(O => O.OrderItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.Where(predicate).ToListAsync();
        }

        public async Task<Order> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
        }

        public void Remove(Order entity)
        {
            _context.Orders.Remove(entity);
        }

        public async Task<(List<Order> Orders, int TotalCount)> SearchAndFilterAsync(
            string searchTerm,
            OrderStatus? status,
            DateTime? dateFrom,
            DateTime? dateTo,
            decimal? minAmount,
            decimal? maxAmount,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Orders
                .Include(O => O.User)
                .Include(O => O.OrderItems)
                .AsQueryable();

            // Search by user name or email
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o =>
                    o.User.FullName.Contains(searchTerm) ||
                    o.User.Email.Contains(searchTerm));
            }

            // Filter by status
            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status);
            }

            // Filter by date range
            if (dateFrom.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= dateFrom);
            }

            if (dateTo.HasValue)
            {
                var endOfDay = dateTo.Value.AddDays(1).AddSeconds(-1);
                query = query.Where(o => o.CreatedAt <= endOfDay);
            }

            // Filter by amount range
            if (minAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= minAmount);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= maxAmount);
            }

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<bool> UpdateOrderStatusAsync(int OrderId, OrderStatus NewStatus)
        {
            Order? order = await _context.Orders
                .Include(O => O.OrderItems)
                    .ThenInclude(OI => OI.OrderItemVariants) 
                .FirstOrDefaultAsync(O => O.Id == OrderId);

            if (order == null || OrderStatus.Cancelled == order.Status)
                return false;

            if (NewStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
            {
                var variantIds = order.OrderItems
                    .SelectMany(OI => OI.OrderItemVariants)
                    .Select(v => v.ProductVariantId)
                    .ToList();

                var variants = await _context.ProductVariants
                    .Where(pv => variantIds.Contains(pv.Id))
                    .ToDictionaryAsync(pv => pv.Id);

                foreach (var orderItem in order.OrderItems)
                {
                    foreach (var itemVariant in orderItem.OrderItemVariants)
                    {
                        if (variants.TryGetValue(itemVariant.ProductVariantId, out var productVariant))
                        {
                            productVariant.Stock += itemVariant.Quantity; 
                        }
                    }
                }
            }

            order.Status = NewStatus;
            await _context.SaveChangesAsync(); 
            return true;
        }
        public async Task<Order?> GetOrderDetailsForAdminAsync(int orderId)
        {
            return await _context.Orders
                .Where(O => O.Id == orderId)
                .Include(O => O.User)
                .Include(O => O.OrderItems)
                    .ThenInclude(OI => OI.Product)
                .Include(O => O.OrderItems)                     
                    .ThenInclude(OI => OI.OrderItemVariants)    
                .FirstOrDefaultAsync();
        }
    }
}
