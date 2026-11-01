using SmartPayPark.Application.OperatorDocuments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Operators.Interfaces.Services
{
    public interface IOperatorDocumentService
    {
        Task<OperatorDocumentResponse> UploadAsync(
        Guid operatorId,
        Guid userId,
        string documentType,
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);
    }
}
