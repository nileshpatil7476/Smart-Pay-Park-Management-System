using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.User;

namespace SmartPayPark.Application.Authentication.Interfaces.Repositories
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
