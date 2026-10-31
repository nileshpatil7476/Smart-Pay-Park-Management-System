
using Microsoft.EntityFrameworkCore;
using SmartPayPark.Infrastructure.Persistence;
using SmartPayPark.Domain.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;

namespace SmartPayPark.Infrastructure.Persistence.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly AppDbContext _context;

    public OperatorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Operator?> GetByIdAsync(Guid operatorId)
    {
        return await _context.Operators
            .Include(x => x.OperatorUsers)
            .Include(x => x.OperatorDocuments)
            .FirstOrDefaultAsync(x => x.Id == operatorId);
    }

    public async Task<Operator?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Operators
            .Where(x => x.OperatorUsers.Any(
                membership =>
                    membership.UserId == userId &&
                    membership.IsActive))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid operatorId)
    {
        return await _context.Operators
            .AnyAsync(x => x.Id == operatorId);
    }

    public async Task AddAsync(Operator operatorEntity)
    {
        await _context.Operators.AddAsync(operatorEntity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
