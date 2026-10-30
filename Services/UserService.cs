using Microsoft.AspNetCore.Identity;
using SmartPayPark.Api.DTOs.User;
using SmartPayPark.Api.Interfaces.IServices;
using SmartPayPark.Api.Interfaces.Repositories;
using SmartPayPark.Api.Interfaces.Services;
using SmartPayPark.Api.Models;

namespace SmartPayPark.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtTokenService) 
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<UserProfileResponse?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserProfileResponse
            {
                Id = userId,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if(user == null || !user.IsActive)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                request.Password);

            if(result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _passwordHasher.HashPassword(
                    user,
                    request.Password);

                await _userRepository.SaveChangesAsync();
            }

            var roles = user.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role!.Name)
                .ToList();

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                AccessToken = token.Token,
                ExpiresAtUtc = token.ExpireAtUtc,
                FullName = user.FullName,
                Email = email,
                Roles = roles,
            };
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                return (false, "Email already registered.");
            }

            // Customer registration assigns only customer.
            var customerRole = await _userRepository
                .GetRoleByNameAsync("Customer");

            if(customerRole == null)
            {
                throw new InvalidOperationException(
                    "Customer role is not configured.");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = email,
                PhoneNumber = request.PhoneNumber.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            user.Password = _passwordHasher.HashPassword(
                user,
                request.Password);

            user.UserRoles.Add(new UserRole
            {
                User = user,
                Role = customerRole,
                RoleId = customerRole.Id
            });

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Registration Successful.");
        }
    }
}
