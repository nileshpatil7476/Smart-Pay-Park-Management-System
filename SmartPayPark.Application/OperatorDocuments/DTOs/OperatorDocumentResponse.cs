using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.OperatorDocuments.DTOs
{
    public class OperatorDocumentResponse
    {
        // generat properties for the response
        public Guid Id { get; set; }
        public Guid OperatorId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string StorageKey { get; set; }  = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public string? VerificationStutus { get; internal set; }
    }
}
