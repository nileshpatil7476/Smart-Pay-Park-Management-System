namespace SmartPayPark.Application.Admin.Operators.DTOs;

public class AdminOperatorDetailsResponse
{
    public Guid Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public string ContactPhone { get; set; } = string.Empty;

    public string? BusinessAddress { get; set; }

    public string VerificationStatus { get; set; } = string.Empty;

    public string? RejectionReason { get; set; }

    public Guid? ReviewedByUserId { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public List<AdminOperatorDocumentResponse> Documents { get; set; } = new();
}

public class AdminOperatorDocumentResponse
{
    public Guid Id { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    public string VerificationStatus { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }
}