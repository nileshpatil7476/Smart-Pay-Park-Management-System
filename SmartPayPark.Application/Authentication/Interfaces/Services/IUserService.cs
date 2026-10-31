using SmartPayPark.Application.Authentication.DTOs;
using SmartPayPark.Domain.Entities.User;

namespace SmartPayPark.Application.Authentication.Interfaces.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<UserProfileResponse?> GetUserByIdAsync(Guid userId);
    }
}
