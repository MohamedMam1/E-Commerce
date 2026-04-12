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

        public async Task SaveOrderAsync(
            string userId,
            List<(int ProductId, int Quantity, decimal Price, ProductSize Size, ProductColor Color)> items)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                OrderItems = items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Size = i.Size,
                    Color = i.Color
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
