using SmartPayPark.Application.Admin.Documents.DTOs;
using SmartPayPark.Application.Admin.Documents.Interfaces.Repositories;
using SmartPayPark.Application.Admin.Documents.Interfaces.Services;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Admin.Documents.Services
{
    public class AdminDocumentService : IAdminDocumentService
    {
        private readonly IAdminDocumentRepository _adminDocumentRepository;

        public AdminDocumentService(IAdminDocumentRepository adminDocumentRepository)
        {
            _adminDocumentRepository = adminDocumentRepository;
        }

        public async Task<AdminOperatorDocumentResponse?> GetDocumentByIdAsync(Guid id)
        {
            var document = await _adminDocumentRepository.GetByIdAsync(id);

            if (document == null)
            {
                return null;
            }

            return MapToResponse(document);
        }

        public async Task<IEnumerable<AdminOperatorDocumentResponse>> GetPendingDocumentsAsync()
        {
            var document = await _adminDocumentRepository.GetPendingDocumentsAsync();

            if (document == null)
            {
                return Enumerable.Empty<AdminOperatorDocumentResponse>();
            }

            return document
                .Select(MapToResponse)
                .Where(response => response != null)!
                .Cast<AdminOperatorDocumentResponse>();
        }

        public async Task<AdminOperatorDocumentResponse> ReviewDocumentAsync(
            Guid id,
            ReviewOperatorDocumentRequest request,
            Guid reviewedByUserId)
        {
            var document = await _adminDocumentRepository.GetByIdAsync(id);

            if (document == null)
            {
                throw new KeyNotFoundException($"Document with ID {id} not found.");
            }

            // check if the document is already reviewed
            if (document.VerificationStatus != DocumentVerificationStatus.Pending)
            {
                throw new InvalidOperationException($"Document with ID {id} has already been reviewed.");
            }

            // check if the verification status is valid
            if (!Enum.TryParse<DocumentVerificationStatus>(
                request.VerificationStatus,
                true,
                out var verificationStatus)
                || verificationStatus == DocumentVerificationStatus.Pending)
            {
                throw new ArgumentException($"Invalid verification status: {request.VerificationStatus}");
            }

            // check if the rejection reason is provided when the status is Rejected
            if (verificationStatus == DocumentVerificationStatus.Rejected 
                && string.IsNullOrWhiteSpace(request.RejectionReason))
            {
                throw new ArgumentException("Rejection reason must be provided when the status is Rejected.");
            }

            // verify the document
            document.VerificationStatus = verificationStatus;
            document.RejectionReason = 
                verificationStatus == DocumentVerificationStatus.Rejected
                    ? request.RejectionReason?.Trim()
                    : null;

            document.ReviewedByUserId = reviewedByUserId;
            document.ReviewedAtUtc = DateTime.UtcNow;

            await _adminDocumentRepository.SaveChangesAsync();

            return MapToResponse(document);
        }

        // private methods
        private AdminOperatorDocumentResponse? MapToResponse(OperatorDocument document)
        {
            return new AdminOperatorDocumentResponse
            {
                Id = document.Id,
                OperatorId = document.OperatorId,
                BusinessName = document.Operator.BusinessName,
                DocumentType = document.DocumentType,
                VerificationStatus = document.VerificationStatus.ToString(),
                RejectionReason = document.RejectionReason,
                CreatedAtUtc = document.CreatedAtUtc,
                ReviewedByUserId = document.ReviewedByUserId,
                ReviewedAtUtc = document.ReviewedAtUtc
            };
        }
    }
}
