using E_Commerce.Interfaces;
using E_Commerce.Services;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class AdminDashboardController : Controller
    {

        private readonly IAdminService _adminService;
        public AdminDashboardController(IAdminService adminService)
           
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/admindashboard")]
        public async Task<IActionResult> AdminDashboard()
        {
            var dashboardDetails =await _adminService.GetDetailsAsync();
            return View(dashboardDetails);
        }


    }
}
