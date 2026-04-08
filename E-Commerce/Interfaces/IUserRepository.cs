using E_Commerce.ViewModels.AdminDashboard;
using FinalProject.Models;

namespace E_Commerce.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserWithAddressesAsync(string UserId);
        Task UpdateUserAddressesAsync(ApplicationUser User, List<string> UpdatedAddresses, string? NewAddressLine);
        Task<List<UserDashBoardVM>> GetUsersWithRolesAsync();

    }
}
