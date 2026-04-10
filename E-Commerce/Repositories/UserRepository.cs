using E_Commerce.Interfaces;
using E_Commerce.ViewModels.AdminDashboard;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ITiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ITiContext Context, UserManager<ApplicationUser> userManager)
        {
            _context = Context;
            _userManager = userManager;
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

        public async Task<UserPaginationVM> GetUsersWithRolesAsync(string? searchTerm, string? status, string? role, int pageNumber, int pageSize)
        {
            IQueryable<ApplicationUser> Query = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string SearchTerm = searchTerm.Trim().ToLower();

                Query = Query.Where(U =>
                    (U.FullName != null && U.FullName.ToLower().Contains(SearchTerm)) ||
                    (U.Email != null && U.Email.ToLower().Contains(SearchTerm)) ||
                    (U.UserName != null && U.UserName.ToLower().Contains(SearchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "Active")
                {
                    Query = Query.Where(U => U.Status == UserStatus.Active);
                }
                else if (status == "Banned")
                {
                    Query = Query.Where(U => U.Status == UserStatus.Banned);
                }
            }

            List<ApplicationUser> Users = await Query
                .OrderByDescending(U => U.CreatedAt)
                .ToListAsync();

            List<UserDashBoardVM> Result = new List<UserDashBoardVM>();

            foreach (ApplicationUser User in Users)
            {
                IList<string> Roles = await _userManager.GetRolesAsync(User);

                Result.Add(new UserDashBoardVM
                {
                    User = User,
                    Roles = Roles.ToList()
                });
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                Result = Result
                    .Where(U => U.Roles.Any(R => R == role))
                    .ToList();
            }

            int TotalCount = Result.Count;

            Result = Result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new UserPaginationVM
            {
                Users = Result,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = TotalCount
            };
        }
        public async Task<bool> IsEmailExists(string email)
        {
            email = email.Trim().ToUpper();
            return await _context.Users.AnyAsync(u => u.NormalizedEmail == email);
        }

    }
}