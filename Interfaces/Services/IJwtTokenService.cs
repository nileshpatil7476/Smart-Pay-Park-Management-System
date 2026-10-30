using Microsoft.IdentityModel.Tokens;
using SmartPayPark.Api.Models;

namespace SmartPayPark.Api.Interfaces.IServices
{
    public interface IJwtTokenService
    {
        (string Token, DateTime ExpireAtUtc) GenerateToken(User user);
        TokenValidationParameters GetValidationParameter();
    }
}
