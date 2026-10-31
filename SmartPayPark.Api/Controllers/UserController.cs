using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPayPark.Application.Authentication.DTOs;
using SmartPayPark.Application.Authentication.Interfaces.Services;
using System.Security.Claims;

namespace SmartPayPark.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.Success)
            {
                return Conflict(new { Message = result.Message });
            }

            return StatusCode(201, new { Message = result.Message});
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            var response = await _userService.LoginAsync(request);

            if(response == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password."
                });
            }

            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(); 
            }

            var response = await _userService.GetUserByIdAsync(userId);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
