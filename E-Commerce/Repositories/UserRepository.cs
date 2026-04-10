using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using E_Commerce.ViewModels.AdminDashboard;

namespace E_Commerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ITiContext _context;

        public UserRepository(ITiContext Context)
        {
            _context = Context;
        }

        public async Task<ApplicationUser?> GetUserWithAddressesAsync(string UserId)
        {
            return await _context.Users
                .Include(U => U.Addresses)
                .FirstOrDefaultAsync(U => U.Id == UserId);
        }

        public async Task UpdateUserAddressesAsync(ApplicationUser User, List<string> UpdatedAddresses, string? NewAddressLine)
        {
            for (int i = 0; i < User.Addresses.Count && i < UpdatedAddresses.Count; i++)
            {
                User.Addresses.ElementAt(i).AddressLine = UpdatedAddresses[i];
            }

            if (!string.IsNullOrWhiteSpace(NewAddressLine))
            {
                User.Addresses.Add(new Address
                {
                    UserId = User.Id,
                    AddressLine = NewAddressLine
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDashBoardVM>> GetUsersWithRolesAsync()
        {
            var usersWithRoles = await (
                from user in _context.Users
                join userRole in _context.UserRoles
                    on user.Id equals userRole.UserId into userRolesGroup
                from userRole in userRolesGroup.DefaultIfEmpty()

                join role in _context.Roles
                    on userRole.RoleId equals role.Id into rolesGroup
                from role in rolesGroup.DefaultIfEmpty()

                group role by user into g

                select new UserDashBoardVM
                {
                    User = g.Key,
                    Roles = g
                        .Where(r => r != null)
                        .Select(r => r.Name)
                        .ToList()
                }
            ).ToListAsync();

            return usersWithRoles;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            email = email.Trim().ToLower();
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email);
        }

    }
}