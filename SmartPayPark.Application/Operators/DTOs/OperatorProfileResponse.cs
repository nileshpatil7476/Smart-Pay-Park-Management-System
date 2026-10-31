
namespace SmartPayPark.Application.Operators.DTOs;

public class OperatorProfileResponse
{
    public Guid Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string BusinessAddress { get; set; } = string.Empty;

    public string VerificationStatus { get; set; } = string.Empty;

    public string? RejectionReason { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }
}
