using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class AdminService : IAdminService
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _usersService;
        public AdminService(
             IProductService productService
            ,IOrderService orderService,
            IUserService usersService)
        {
            _productService = productService;
            _orderService = orderService;
            _usersService = usersService;
        }

        public async Task<AdminDashboardDetailVM> GetDetailsAsync()
        {
            var usersWithRoles = await _usersService.GetUsersWithRolesAsync();

            return new AdminDashboardDetailVM
            {
                Users = usersWithRoles
            };
        }
    }
}
