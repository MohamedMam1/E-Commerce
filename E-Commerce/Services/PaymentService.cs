using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
     List<(int ProductVariantId, int Quantity, decimal Price)> items)
        {
            var variantIds = items.Select(i => i.ProductVariantId).ToList();

            var variants = _context.ProductVariants
                .Where(pv => variantIds.Contains(pv.Id))
                .ToDictionary(pv => pv.Id);

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                TotalAmount = items.Sum(i => i.Price * i.Quantity),
                OrderItems = items.Select(i =>
                {
                    if (!variants.TryGetValue(i.ProductVariantId, out var variant))
                        throw new InvalidOperationException(
                            $"ProductVariant with Id {i.ProductVariantId} not found.");

                    if (variant.Stock < i.Quantity)
                        throw new InvalidOperationException(
                            $"Not enough stock for variant {i.ProductVariantId}.");

                    variant.Stock -= i.Quantity;

                    return new OrderItem
                    {
                        ProductId = variant.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        OrderItemVariants = new List<OrderItemVariant>
                {
                    new OrderItemVariant
                    {
                        ProductVariantId = i.ProductVariantId,
                        Quantity = i.Quantity,
                        Size = variant.Size,
                        Color = variant.Color
                    }
                }
                    };
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); 
        }
    }
}

