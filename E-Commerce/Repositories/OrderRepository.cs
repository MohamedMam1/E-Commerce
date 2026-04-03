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
            return await _context.Orders.ToListAsync();
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
    }
}
