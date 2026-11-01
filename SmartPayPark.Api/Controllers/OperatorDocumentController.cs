using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPayPark.Application.OperatorDocuments.DTOs;
using SmartPayPark.Application.Operators.Interfaces;
using SmartPayPark.Application.Operators.Interfaces.Services;
using System.Security.Claims;

namespace SmartPayPark.Api.Controllers;

[ApiController]
[Route("api/v1/operators/{operatorId:guid}/documents")]
[Authorize]
public class OperatorDocumentController : ControllerBase
{
    private readonly IOperatorDocumentService _documentService;

    public OperatorDocumentController(
        IOperatorDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadDocument(
        Guid operatorId,
        [FromForm] UploadDocumentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest(new
            {
                Message = "Please select a document to upload."
            });
        }

        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        try
        {
            await using var stream = request.File.OpenReadStream();

            var result = await _documentService.UploadAsync(
                operatorId,
                userId,
                request.DocumentType,
                stream,
                request.File.FileName,
                cancellationToken);

            return CreatedAtAction(
                nameof(UploadDocument),
                new { operatorId },
                result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "Operator not found." });
        }
    }
}