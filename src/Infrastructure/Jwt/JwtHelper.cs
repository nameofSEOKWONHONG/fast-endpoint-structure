using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Jwt;

public class JwtHelper
{
    public const string Issuer = "FastEndpointApi";
    public const string Audience = "FastEndpointApi";
    public const string SecretKey = "aHb2c9YRGSlFZDaPfbA3m4z+Lyr4O9dSWL2c+CiqWRM=";
    
    public static string GenerateJwtToken(DateTime? expire, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: expire,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}