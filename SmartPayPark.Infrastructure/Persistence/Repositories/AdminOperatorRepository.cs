using Microsoft.EntityFrameworkCore;

using SmartPayPark.Application.Admin.Operators.Interfaces;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using SmartPayPark.Infrastructure.Persistence;

namespace SmartPayPark.Infrastructure.Persistence.Repositories;

public class AdminOperatorRepository : IAdminOperatorRepository
{
    private readonly AppDbContext _context;

    public AdminOperatorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operator>>
        GetPendingOperatorsAsync()
    {
        return await _context.Operators
            .AsNoTracking()
            .Where(x => x.VerificationStatus == OperatorStatus.Pending)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<IEnumerable<Operator>>
        GetAllOperatorsAsync()
    {
        return await _context.Operators
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Operator?> GetByIdAsync(Guid id)
    {
        return await _context.Operators
            .Include(x => x.OperatorUsers)
                .ThenInclude(x => x.User)
            .Include(x => x.OperatorDocuments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}