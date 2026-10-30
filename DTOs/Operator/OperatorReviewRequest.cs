
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.Operator;

public class OperatorReviewRequest
{
    [Required]
    [RegularExpression(
        "Approved|Rejected|Suspended",
        ErrorMessage = "Status must be Approved, Rejected, or Suspended.")]
    public string VerificationStatus { get; set; } = string.Empty;

    [StringLength(500)]
    public string? RejectionReason { get; set; }
}
