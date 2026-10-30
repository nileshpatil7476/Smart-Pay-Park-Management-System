using SmartPayPark.Api.DTOs.User;

namespace SmartPayPark.Api.Interfaces.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<UserProfileResponse?> GetUserByIdAsync(Guid userId);
    }
}
