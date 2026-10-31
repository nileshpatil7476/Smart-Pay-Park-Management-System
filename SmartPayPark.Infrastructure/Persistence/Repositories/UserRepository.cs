using Microsoft.EntityFrameworkCore;
using SmartPayPark.Infrastructure.Persistence;
using SmartPayPark.Domain.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Application.Authentication.Interfaces.Repositories;
using SmartPayPark.Domain.Entities.User;

namespace SmartPayPark.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var user = await GetByIdAsync(id);

            if (user != null)
            {
                user.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var email = user.Email.Trim().ToLowerInvariant();

            var existingUser = await GetByEmailAsync(email);

            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Password = user.Password;
                existingUser.UserRoles = user.UserRoles;
                existingUser.UpdatedAt = DateTime.UtcNow;
                existingUser.PhoneNumber = user.PhoneNumber;
            }

            await AddAsync(user);

            await SaveChangesAsync();
        }
    }
}
