using SmartPayPark.Api.DTOs;
using SmartPayPark.Api.Models;

namespace SmartPayPark.Api.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
        Task DeleteByIdAsync(Guid id);
    }
}
