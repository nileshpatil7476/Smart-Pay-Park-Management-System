using SmartPayPark.Domain.Entities;

namespace SmartPayPark.Application.Admin.Operators.Interfaces;

public interface IAdminOperatorRepository
{
    Task<IEnumerable<Operator>> GetPendingOperatorsAsync();

    Task<IEnumerable<Operator>> GetAllOperatorsAsync();

    Task<Operator?> GetByIdAsync(Guid id);

    Task SaveChangesAsync();
}