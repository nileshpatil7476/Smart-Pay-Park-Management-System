using SmartPayPark.Domain.Entities;

namespace SmartPayPark.Domain.Interfaces.Repositories
{
    public interface IOperatorRepository
    {
        Task<Operator?> GetByIdAsync(Guid operatorId);

        Task<Operator?> GetByUserIdAsync(Guid userId);

        Task<bool> ExistsAsync(Guid operatorId);

        Task AddAsync(Operator operatorEntity);

        Task SaveChangesAsync();
    }
}
