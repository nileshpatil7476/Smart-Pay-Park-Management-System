using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Admin.Documents.DTOs
{
    public class ReviewOperatorDocumentRequest
    {
        [Required]
        [RegularExpression(
        "^(Approved|Rejected)$",
        ErrorMessage = "Status must be Approved or Rejected.")]
        public string VerificationStatus { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }
    }
}
