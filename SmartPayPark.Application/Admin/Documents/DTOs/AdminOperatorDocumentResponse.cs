using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.Admin.Documents.DTOs
{
    public class AdminOperatorDocumentResponse
    {
        public Guid Id { get; set; }

        public Guid OperatorId { get; set; }

        public string BusinessName { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public string VerificationStatus { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Guid? ReviewedByUserId { get; set; }

        public DateTime? ReviewedAtUtc { get; set; }
    }
}
