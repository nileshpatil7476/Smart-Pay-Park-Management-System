using SmartPayPark.Application.Admin.Operators.DTOs;
using SmartPayPark.Application.Admin.Operators.Interfaces;
using SmartPayPark.Application.Operators.DTOs;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;

namespace SmartPayPark.Application.Admin.Operators.Services;

public class AdminOperatorService : IAdminOperatorService
{
    private readonly IAdminOperatorRepository _repository;

    public AdminOperatorService(IAdminOperatorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OperatorResponse>>
        GetPendingOperatorsAsync()
    {
        var operators = await _repository.GetPendingOperatorsAsync();

        return operators.Select(MapToOperatorResponse);
    }

    public async Task<IEnumerable<OperatorResponse>>
        GetAllOperatorsAsync()
    {
        var operators = await _repository.GetAllOperatorsAsync();

        return operators.Select(MapToOperatorResponse);
    }

    public async Task<AdminOperatorDetailsResponse?>
        GetOperatorByIdAsync(Guid id)
    {
        var operatorEntity = await _repository.GetByIdAsync(id);

        if (operatorEntity == null)
            return null;

        return MapToDetailsResponse(operatorEntity);
    }

    public async Task<AdminOperatorDetailsResponse> ReviewOperatorAsync(
        Guid id,
        OperatorReviewRequest request,
        Guid reviewedByUserId)
    {
        var operatorEntity = await _repository.GetByIdAsync(id);

        if (operatorEntity == null)
            throw new KeyNotFoundException("Operator not found.");

        if (operatorEntity.VerificationStatus != OperatorStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending operators can be reviewed.");
        }

        if (!Enum.TryParse<OperatorStatus>(
                request.VerificationStatus,
                true,
                out var status))
        {
            throw new InvalidOperationException(
                "Invalid operator verification status.");
        }

        if (status == OperatorStatus.Pending)
        {
            throw new InvalidOperationException(
                "Pending is not a valid review decision.");
        }

        if (status == OperatorStatus.Rejected &&
            string.IsNullOrWhiteSpace(request.RejectionReason))
        {
            throw new InvalidOperationException(
                "Rejection reason is required.");
        }

        operatorEntity.VerificationStatus = status;
        operatorEntity.RejectionReason =
            status == OperatorStatus.Rejected
                ? request.RejectionReason?.Trim()
                : null;

        operatorEntity.ReviewedByUserId = reviewedByUserId;
        operatorEntity.ReviewedAtUtc = DateTime.UtcNow;
        operatorEntity.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return MapToDetailsResponse(operatorEntity);
    }

    private static OperatorResponse MapToOperatorResponse(
        Operator operatorEntity)
    {
        return new OperatorResponse
        {
            Id = operatorEntity.Id,
            BusinessName = operatorEntity.BusinessName,
            VerificationStatus =
                operatorEntity.VerificationStatus.ToString(),
            CreatedAtUtc = operatorEntity.CreatedAtUtc
        };
    }

    private static AdminOperatorDetailsResponse MapToDetailsResponse(
        Operator operatorEntity)
    {
        return new AdminOperatorDetailsResponse
        {
            Id = operatorEntity.Id,
            BusinessName = operatorEntity.BusinessName,
            ContactName = operatorEntity.ContactName,
            ContactEmail = operatorEntity.ContactEmail,
            ContactPhone = operatorEntity.ContactPhone,
            BusinessAddress = operatorEntity.BusinessAddress,
            VerificationStatus =
                operatorEntity.VerificationStatus.ToString(),
            RejectionReason = operatorEntity.RejectionReason,
            ReviewedByUserId = operatorEntity.ReviewedByUserId,
            ReviewedAtUtc = operatorEntity.ReviewedAtUtc,
            CreatedAtUtc = operatorEntity.CreatedAtUtc,

            Documents = operatorEntity.OperatorDocuments
                .Select(document => new AdminOperatorDocumentResponse
                {
                    Id = document.Id,
                    DocumentType = document.DocumentType,
                    VerificationStatus =
                        document.VerificationStatus.ToString(),
                    CreatedAtUtc = document.CreatedAtUtc,
                    ReviewedAtUtc = document.ReviewedAtUtc
                })
                .ToList()
        };
    }
}