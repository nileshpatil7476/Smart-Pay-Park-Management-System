using SmartPayPark.Domain.Entities.Enums;

namespace SmartPayPark.Domain.Entities;

public class OperatorDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OperatorId { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    // Private file storage reference, not the actual file
    public string StorageKey { get; set; } = string.Empty;

    public DocumentVerificationStatus VerificationStatus { get; set; }
        = DocumentVerificationStatus.Pending;

    public Guid? ReviewedByUserId { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Operator Operator { get; set; } = null!;
}