using SmartPayPark.Domain.Entities.Enums;

namespace SmartPayPark.Domain.Entities;

public class Operator
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string BusinessName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public string ContactPhone { get; set; } = string.Empty;

    public string? BusinessAddress { get; set; }

    public OperatorStatus VerificationStatus { get; set; }
        = OperatorStatus.Pending;

    public Guid? ReviewedByUserId { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    // Navigation properties
    public ICollection<OperatorUser> OperatorUsers { get; set; }
        = new List<OperatorUser>();

    public ICollection<OperatorDocument> OperatorDocuments { get; set; }
        = new List<OperatorDocument>();
}