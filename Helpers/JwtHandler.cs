using _1670_API.Models;
using _1670_API.Models.DTOs;
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
        public static string GenerateToken(User user)
        {
            var content = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
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
        public static UserDTO? ValiateToken(HttpContext httpContext)
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

            UserDTO userDTO = new()
            {
                Id = int.Parse(tokenContent.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = tokenContent.FindFirstValue(ClaimTypes.Email),
                Role = tokenContent.FindFirstValue(ClaimTypes.Role),
            };
            return userDTO;
        }
    }
}
