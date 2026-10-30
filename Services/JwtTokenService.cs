using Microsoft.IdentityModel.Tokens;
using SmartPayPark.Api.Interfaces.IServices;
using SmartPayPark.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartPayPark.Api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpireAtUtc) GenerateToken(User user)
        {
            var key = _configuration["Jwt:key"]
                ?? throw new InvalidOperationException("JWT signing Key is Not Configured.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuuer is not configured.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured.");

            var keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length < 32)
            {
                throw new InvalidOperationException("JWT signing key is not valid");
            }

            var expireAt = DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("Jwt:AccessTokenMinutes", 30));

            var claim = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var userRole in user.UserRoles)
            {
                claim.Add(new Claim(
                    ClaimTypes.Role, userRole.Role.Name));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer:issuer,
                audience:audience,
                claims:claim,
                expires:expireAt,
                signingCredentials:credentials);

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expireAt);

            throw new NotImplementedException();
        }

        public TokenValidationParameters GetValidationParameter()
        {
            var key = _configuration["Jwt:key"]
                ?? throw new InvalidOperationException("JWT signing Key is Not Configured.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuuer is not configured.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured.");

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),

                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };
        }
    }
}
