using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;

namespace E_Commerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ITiContext _context;

        public PaymentService(ITiContext context)
        {
            _context = context;
        }

        public async Task SaveOrderAsync(string userId, List<(int ProductId, int Quantity, decimal Price)> items)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Completed,
                TotalAmount = items.Sum(i => i.Quantity * i.Price),
                OrderItems = items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity = Math.Max(0, product.Quantity - item.Quantity);
                    _context.Products.Update(product);
                }
            }

            var cartItems = _context.Carts.Where(c => c.UserId == userId);
            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
        }
    }
}