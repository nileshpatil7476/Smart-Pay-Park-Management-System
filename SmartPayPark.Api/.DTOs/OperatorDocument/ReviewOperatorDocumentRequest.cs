
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.OperatorDocument;

public class ReviewOperatorDocumentRequest
{
    [Required]
    [RegularExpression("Approved|Rejected")]
    public string VerificationStatus { get; set; } = string.Empty;

    [StringLength(500)]
    public string? RejectionReason { get; set; }
}
