using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartPayPark.Application.Admin.Operators.DTOs;
using SmartPayPark.Application.Admin.Operators.Interfaces;
using SmartPayPark.Application.Operators.DTOs;

namespace SmartPayPark.Api.Controllers;

[Route("api/v1/admin/operators")]
[ApiController]
[Authorize(Roles = "PlatformAdmin")]
public class AdminOperatorController : ControllerBase
{
    private readonly IAdminOperatorService _adminOperatorService;

    public AdminOperatorController(
        IAdminOperatorService adminOperatorService)
    {
        _adminOperatorService = adminOperatorService;
    }

    // GET: api/v1/admin/operators/pending
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingOperators()
    {
        var result =
            await _adminOperatorService.GetPendingOperatorsAsync();

        return Ok(result);
    }

    // GET: api/v1/admin/operators
    [HttpGet]
    public async Task<IActionResult> GetAllOperators()
    {
        var result =
            await _adminOperatorService.GetAllOperatorsAsync();

        return Ok(result);
    }

    // GET: api/v1/admin/operators/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOperatorById(Guid id)
    {
        var result =
            await _adminOperatorService.GetOperatorByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                Message = "Operator not found."
            });
        }

        return Ok(result);
    }

    // PUT: api/v1/admin/operators/{id}/review
    [HttpPut("{id:guid}/review")]
    public async Task<IActionResult> ReviewOperator(
        Guid id,
        [FromBody] OperatorReviewRequest request)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdClaim, out var reviewedByUserId))
        {
            return Unauthorized(new
            {
                Message = "Invalid admin user information."
            });
        }

        try
        {
            var result = await _adminOperatorService.ReviewOperatorAsync(
                id,
                request,
                reviewedByUserId);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                Message = "Operator not found."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }
}