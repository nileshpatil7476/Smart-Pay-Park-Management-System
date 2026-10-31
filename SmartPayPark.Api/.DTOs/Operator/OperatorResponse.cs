
namespace SmartPayPark.Api.DTOs.Operator;

public class OperatorResponse
{
    public Guid Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string VerificationStatus { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
