using SmartPayPark.Application.Common.Interfaces;
using SmartPayPark.Application.OperatorDocuments.DTOs;
using SmartPayPark.Application.Operators.Interfaces.Repositories;
using SmartPayPark.Application.Operators.Interfaces.Services;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Operators.Services
{
    public class OperatorDocumentService : IOperatorDocumentService
    {
        private readonly IOperatorDocumentRepository _operatorDocumentRepository;
        private readonly IFileStorageService _fileStorageService;

        private static readonly HashSet<string> AllowedDocumentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "BusinessLicense",
            "InsuranceCertificate",
            "TaxDocument",
            "AadharCard",
            "PANCard",
            "GSTCertificate",
            "Other"
        };

        public OperatorDocumentService(
            IOperatorDocumentRepository operatorDocumentRepository,
            IFileStorageService fileStorageService)
        {
            _operatorDocumentRepository = operatorDocumentRepository;
            _fileStorageService = fileStorageService;
        }
        public async Task<OperatorDocumentResponse> UploadAsync(
            Guid operatorId, 
            Guid userId, 
            string documentType, 
            Stream fileStream, 
            string fileName, 
            CancellationToken cancellationToken = default)
        {
            // check if the document type is allowed
            if (!AllowedDocumentTypes.Contains(documentType))
            {
                throw new ArgumentException($"Invalid document type: {documentType}. Allowed types are: {string.Join(", ", AllowedDocumentTypes)}");
            }

            // check can upload document
            var canUpload = await _operatorDocumentRepository.CanUploadDocumentAsync(operatorId, userId);

            if(!canUpload)
            {
                throw new InvalidOperationException("User is not authorized to upload documents for this operator.");
            }

            // get extension
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            // if extension is not allowed, throw exception
            if (extension != ".pdf" && extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                throw new ArgumentException($"Invalid file extension: {extension}. Allowed extensions are: .pdf, .jpg, .jpeg, .png");
            }

            // check file size, if greater than 5MB, throw exception
            if (fileStream.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException($"File size exceeds the limit of 5MB.");
            }

            var storageKey = await _fileStorageService.UploadAsync(
                fileStream, 
                fileName, 
                cancellationToken);

            try
            {
                var document = new OperatorDocument
                {
                    Id = Guid.NewGuid(),
                    OperatorId = operatorId,
                    DocumentType = documentType,
                    StorageKey = storageKey,
                    VerificationStatus = DocumentVerificationStatus.Pending,
                    CreatedAtUtc = DateTime.UtcNow
                };

                await _operatorDocumentRepository.AddAsync(document);
                await _operatorDocumentRepository.SaveChangesAsync();

                return new OperatorDocumentResponse
                {
                    Id = document.Id,
                    OperatorId = document.OperatorId,
                    DocumentType = document.DocumentType,
                    StorageKey = document.StorageKey,
                    VerificationStutus = document.VerificationStatus.ToString(),
                    CreatedAtUtc = document.CreatedAtUtc
                };
            }
            catch
            {
                await _fileStorageService.DeleteAsync(storageKey, cancellationToken);

                throw;
            }
        }
    }
}
