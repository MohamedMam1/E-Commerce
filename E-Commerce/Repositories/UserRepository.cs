using E_Commerce.Interfaces;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Users.Include(U => U.Addresses).FirstOrDefaultAsync(U => U.Id == UserId);
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
    }
}
