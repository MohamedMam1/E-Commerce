using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminViewModel.Dashboard;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_Commerce.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITiContext _context;
        public AdminDashboardService(
            UserManager<ApplicationUser> userManager,
            ITiContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<AdminDashboardVM> GetDashBoardDetails()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.Select(o => (decimal?)o.TotalAmount).SumAsync() ?? 0;
            var activeOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Processing);
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
            var ordersLastWeek = await _context.Orders.CountAsync(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-7));
            var totalProducts = await _context.Products.CountAsync();
            var latestOrders = await _context.Orders.OrderByDescending(o => o.CreatedAt).Take(5)
                .Select(o => new OrderDashboardVM
                {
                    Id = o.Id,
                    CustomerName = o.User.FullName,
                    Amount = o.TotalAmount,
                    OrderStatus = o.Status
                })
                .ToListAsync();

            var topProducts = await _context.Products.OrderByDescending(p => p.Quantity).Take(5)
                .Select(p => new ProductDashboardVM
                {
                    ProductName = p.Name,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Stock = p.Quantity
                })
                .ToListAsync();

            return new AdminDashboardVM
            {
                TotalUsers = totalUsers,
                Totalorders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalProducts = totalProducts,
                AverageOrderValue = totalOrders == 0 ? 0: totalRevenue / totalOrders,
                ActiveOrders = activeOrders,
                PendingOrders = pendingOrders, 
                OrdersLastweek = ordersLastWeek,
                Orders = latestOrders,
                Products = topProducts
            };
        }

    }
}
