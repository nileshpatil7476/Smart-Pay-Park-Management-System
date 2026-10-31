using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPayPark.Application.Authentication;
using System.Security.Claims;
using SmartPayPark.Application.Operators.Interfaces.Services;
using SmartPayPark.Application.Operators.DTOs;

namespace SmartPayPark.Api.Controllers
{
    [ApiController]
    [Route("api/v1/operators")]
    [Authorize]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operatorService;

        public OperatorController(
            IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfileAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user authontication token."
                });
            }

            var response = await _operatorService.GetMyProfileAsync(userId);

            if (response == null) {
                return NotFound(new
                {
                    Message = "Operator profile not found."
                });
            }

            return Ok(response);
        }
             
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] CreateOperatorRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userIdClaim, out var userId)) {
                return Unauthorized(new
                {
                    Message = "Invalid user authentication token"
                });
            }

            var response = await _operatorService.RegisterAsync(request, userId);

            return Created(nameof(GetMyProfileAsync), response);
        }
    }
}
