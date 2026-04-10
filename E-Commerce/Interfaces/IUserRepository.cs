using E_Commerce.ViewModels.AdminDashboard;
using FinalProject.Models;

namespace E_Commerce.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserWithAddressesAsync(string UserId);
        Task UpdateUserAddressesAsync(ApplicationUser User, List<string> UpdatedAddresses, string? NewAddressLine);
        Task<List<UserDashBoardVM>> GetUsersWithRolesAsync();
        Task<bool> IsEmailExists(string email);
        Task<UserPaginationVM> GetUsersWithRolesAsync(string? searchTerm, string? status, string? role, int pageNumber, int pageSize);



    }
}
