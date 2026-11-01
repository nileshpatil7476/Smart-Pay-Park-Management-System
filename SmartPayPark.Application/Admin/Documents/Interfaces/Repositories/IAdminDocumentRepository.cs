using SmartPayPark.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Admin.Documents.Interfaces.Repositories
{
    public interface IAdminDocumentRepository
    {
        Task<IEnumerable<OperatorDocument>> GetPendingDocumentsAsync();

        Task<OperatorDocument?> GetByIdAsync(Guid id);

        Task SaveChangesAsync();
    }
}
