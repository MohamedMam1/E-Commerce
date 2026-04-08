using E_Commerce.ViewModels.AdminDashboard;

namespace E_Commerce.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDetailVM> GetDetailsAsync();
    }
}
