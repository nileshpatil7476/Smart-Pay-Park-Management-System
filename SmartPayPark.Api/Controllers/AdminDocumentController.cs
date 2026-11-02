using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPayPark.Application.Admin.Documents.DTOs;
using SmartPayPark.Application.Admin.Documents.Interfaces;
using SmartPayPark.Application.Admin.Documents.Interfaces.Services;
using SmartPayPark.Application.Admin.Documents.Services;
using System.Security.Claims;

namespace SmartPayPark.Api.Controllers;

[Route("api/v1/admin/documents")]
[ApiController]
[Authorize(Roles = "PlatformAdmin")]
public class AdminDocumentController : ControllerBase
{
    private readonly IAdminDocumentService _service;

    public AdminDocumentController(IAdminDocumentService service)
    {
        _service = service;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingDocuments()
    {
        var result = await _service.GetPendingDocumentsAsync();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDocumentById(Guid id)
    {
        var result = await _service.GetDocumentByIdAsync(id);

        if (result == null)
            return NotFound(new { Message = "Document not found." });

        return Ok(result);
    }

    [HttpPut("{id:guid}/review")]
    public async Task<IActionResult> ReviewDocument(
        Guid id,
        [FromBody] ReviewOperatorDocumentRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdClaim, out var reviewedByUserId))
            return Unauthorized();

        try
        {
            var result = await _service.ReviewDocumentAsync(
                id,
                request,
                reviewedByUserId);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "Document not found." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/file")]
    public async Task<IActionResult> DownloadDocument(
    Guid id,
    CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.DownloadDocumentAsync(
                id,
                cancellationToken);

            Response.Headers["Content-Disposition"] =
                $"inline; filename=\"{result.FileName}\"";

            return File(result.FileStream, result.ContentType);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = "Document not found."
            });
        }
        catch (FileNotFoundException)
        {
            return NotFound(new
            {
                message = "The uploaded document file could not be found."
            });
        }
    }
}