using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BlazorSecretManager.Infrastructure;

public class JwtGenerator
{
    public static string GenerateJwtToken(DateTime? expire, string id, string email, string name, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role ?? string.Empty),
        };

        var secretKey = "PEDOSriPgbjP45poei5ghyjoPOID24fthgbhEo5RETYGrdft";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "localhost:4900",
            audience: "localhost:4900",
            claims: claims,
            expires: expire,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}