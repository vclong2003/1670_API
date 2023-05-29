using _1670_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _1670_API.Helpers
{
    public class JwtHandler
    {
        private static readonly JwtSecurityTokenHandler _handler = new();
        private static readonly SymmetricSecurityKey _secretKey = new(Encoding.ASCII.GetBytes("testMyKeyVCLsdjflksjdlkfsjdklhdsjghkjdfhgjdfhjsa")); // will move to global later
        private static readonly DateTime _expire = DateTime.UtcNow.AddDays(1);

        // Generates a JWT token based on the provided user information
        public static string GenerateToken(Account account)
        {
            var content = new[] {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
            };

            var tokenParameters = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(content),
                Expires = _expire,
                SigningCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _handler.CreateToken(tokenParameters);
            return _handler.WriteToken(token);
        }

        // Validates and extracts user information from a JWT token stored in the HTTP context's cookie
        public static AccountDTO? ValiateToken(HttpContext httpContext)
        {
            string? token = httpContext.Request.Cookies["token"];

            if (token == null) { return null; }

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = _secretKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            ClaimsPrincipal tokenContent;

            try
            {
                tokenContent = _handler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            AccountDTO accountDTO = new()
            {
                Id = int.Parse(tokenContent.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = tokenContent.FindFirstValue(ClaimTypes.Email),
                Role = tokenContent.FindFirstValue(ClaimTypes.Role),
            };
            return accountDTO;
        }
    }
}
