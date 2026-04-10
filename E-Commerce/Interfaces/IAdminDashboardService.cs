using E_Commerce.ViewModels.AdminViewModel.Dashboard;
using FinalProject.Models;

namespace E_Commerce.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardVM> GetDashBoardDetails();
    }
}
