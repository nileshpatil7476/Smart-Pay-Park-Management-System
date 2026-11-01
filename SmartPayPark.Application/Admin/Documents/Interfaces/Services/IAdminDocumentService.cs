using SmartPayPark.Application.Admin.Documents.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Admin.Documents.Interfaces.Services
{
    public interface IAdminDocumentService
    {
        Task<IEnumerable<AdminOperatorDocumentResponse>> GetPendingDocumentsAsync();

        Task<AdminOperatorDocumentResponse?> GetDocumentByIdAsync(Guid id);

        Task<AdminOperatorDocumentResponse> ReviewDocumentAsync(
                Guid id,
                ReviewOperatorDocumentRequest request,
                Guid reviewedByUserId);
    }
}
