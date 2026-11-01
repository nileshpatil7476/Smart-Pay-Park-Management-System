using SmartPayPark.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Operators.Interfaces.Repositories
{
    public interface IOperatorDocumentRepository
    {
        Task<bool> CanUploadDocumentAsync(
            Guid operatorId,
            Guid userId);

        Task AddAsync(OperatorDocument document);

        Task SaveChangesAsync();
    }
}
