using Microsoft.EntityFrameworkCore;
using SmartPayPark.Application.Operators.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Infrastructure.Persistence.Repositories
{
    public class OperatorDocumentRepository : IOperatorDocumentRepository
    {
        private readonly AppDbContext _context;

        public OperatorDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OperatorDocument document)
        {
            await _context.OperatorDocuments.AddAsync(document);
        }

        public async Task<bool> CanUploadDocumentAsync(
            Guid operatorId,
            Guid userId)
        {
            return await _context.OperatorUsers
                .AnyAsync(s =>
                    s.OperatorId == operatorId &&
                    s.UserId == userId &&
                    s.IsActive &&
                    (s.OperatorRole == OperatorRole.Owner ||
                    s.OperatorRole == OperatorRole.Manager));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
