using Microsoft.EntityFrameworkCore;
using SmartPayPark.Application.Admin.Documents.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Infrastructure.Persistence.Repositories
{
    public class AdminDocumentRepository : IAdminDocumentRepository
    {
        private readonly AppDbContext _context;

        public AdminDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperatorDocument?> GetByIdAsync(Guid id)
        {
            return await _context.OperatorDocuments
                .Include(od => od.Operator)
                .FirstOrDefaultAsync(od => od.Operator.Id == id);
        }

        public async Task<IEnumerable<OperatorDocument>> GetPendingDocumentsAsync()
        {
            return await _context.OperatorDocuments
                .Include(x => x.Operator)
                .Where(x => x.VerificationStatus == DocumentVerificationStatus.Pending)
                .OrderBy(x => x.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
