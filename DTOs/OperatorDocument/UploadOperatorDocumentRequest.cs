
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SmartPayPark.Api.DTOs.OperatorDocument;

public class UploadOperatorDocumentRequest
{
    [Required]
    [StringLength(50)]
    public string DocumentType { get; set; } = string.Empty;

    [Required]
    public IFormFile File { get; set; } = null!;
}
