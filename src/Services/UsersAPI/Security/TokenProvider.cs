using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using UsersAPI.Models;

namespace UsersAPI.Security
{
    public sealed class TokenProvider(IConfiguration configuration)
    {
        public string Create(User user)
        {
            string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? configuration["Jwt:Secret"] ?? "TemporaryDefaultSecretKeyForDevelopmentOnly123!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                    ]),
                Expires = DateTime.UtcNow.AddHours(60),
                SigningCredentials = credentials,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? configuration["Jwt:Issuer"],
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
        
    }
}
