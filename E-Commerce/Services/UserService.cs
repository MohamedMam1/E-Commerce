using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
using E_Commerce.ViewModels.UserDashboard;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository)
        {
            _userRepository = UserRepository;
        }

        public async Task<List<UserAddressVM>> GetUserAddressesAsync(string UserId)
        {
            ApplicationUser? User = await _userRepository.GetUserWithAddressesAsync(UserId);

            if (User == null || User.Addresses == null)
            {
                return new List<UserAddressVM>();
            }

            return User.Addresses
                .Select(A => new UserAddressVM
                {
                    Id = A.Id,
                    AddressLine = A.AddressLine
                })
                .ToList();
        }

        public async Task UpdateUserAddressesAsync(string UserId, List<UserAddressVM> Addresses, string? NewAddressLine)
        {
            ApplicationUser? User = await _userRepository.GetUserWithAddressesAsync(UserId);

            if (User == null)
            {
                return;
            }

            List<string> UpdatedAddresses = Addresses
                .Select(A => A.AddressLine)
                .ToList();

            await _userRepository.UpdateUserAddressesAsync(User, UpdatedAddresses, NewAddressLine);
        }
        public async Task<UserPaginationVM> GetUsersWithRolesAsync(string? searchTerm, string? status, string? role, int pageNumber, int pageSize)
        {
            return await _userRepository.GetUsersWithRolesAsync(searchTerm, status, role, pageNumber, pageSize);
        }
    }
}
