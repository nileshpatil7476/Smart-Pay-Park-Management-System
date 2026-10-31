using Microsoft.IdentityModel.Tokens;
using SmartPayPark.Domain.Entities.User;

namespace SmartPayPark.Application.Authentication.Interfaces.Services
{
    public interface IJwtTokenService
    {
        (string Token, DateTime ExpireAtUtc) GenerateToken(User user);
        TokenValidationParameters GetValidationParameter();
    }
}
