
namespace SmartPayPark.Api.DTOs.OperatorDocument;

public class OperatorDocumentResponse
{
    public Guid Id { get; set; }

    public Guid OperatorId { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    public string VerificationStatus { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }
}
