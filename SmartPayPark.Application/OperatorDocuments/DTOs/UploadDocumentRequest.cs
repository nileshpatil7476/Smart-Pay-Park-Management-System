using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPayPark.Application.OperatorDocuments.DTOs
{
    public class UploadDocumentRequest
    {
        public string DocumentType { get; set; } = string.Empty;

        public IFormFile File { get; set; } = null!;
    }
}
