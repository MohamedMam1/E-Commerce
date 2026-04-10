using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.UserDashboard;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface IUserService
    {
        Task<List<UserAddressVM>> GetUserAddressesAsync(string UserId);
        Task UpdateUserAddressesAsync(string UserId, List<UserAddressVM> Addresses, string? NewAddressLine);
        Task<UserPaginationVM> GetUsersWithRolesAsync(string? searchTerm, string? status, string? role, int pageNumber, int pageSize);
    }
}
