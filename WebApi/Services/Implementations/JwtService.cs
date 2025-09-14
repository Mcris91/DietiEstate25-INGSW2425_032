using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DietiEstate.Shared.Models.AuthModels;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi.Services.Implementations;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private const string SecretKey = "aaa";
    private const string Issuer = "aaa";
    private const string Audience = "aaa";
    private const int AccessTokenExpiryInMinutes = 30;
    private const int RefreshTokenExpiryInDays = 7;
    
    public string GenerateJwtAccessToken(string userId)
    {
        return GenerateJwtToken(userId, JwtTokenType.Access, AccessTokenExpiryInMinutes);
    }
    public string GenerateJwtRefreshToken(string userId)
    {
        return GenerateJwtToken(userId, JwtTokenType.Refresh, RefreshTokenExpiryInDays * 24 * 60);
    }
    private static string GenerateJwtToken(string userId, JwtTokenType tokenType, int tokenExpiryInMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, 
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                ClaimValueTypes.Integer64),
            new("tokenType", tokenType.ToString().ToLower())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes),
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal? ValidateJwtAccessToken(string token)
    {
        var principal = ValidateJwtToken(token);
        if (principal is null) return null;
        
        return (principal!.FindFirst("token_type")?.Value)!.Equals(nameof(JwtTokenType.Access), StringComparison.CurrentCultureIgnoreCase) 
            ? principal 
            : null;
    }
    public ClaimsPrincipal? ValidateJwtRefreshToken(string token)
    {
        var principal = ValidateJwtToken(token);
        if (principal is null) return null;
        
        return (principal!.FindFirst("token_type")?.Value)!.Equals(nameof(JwtTokenType.Refresh), StringComparison.CurrentCultureIgnoreCase) 
            ? principal 
            : null;
    }
    private static ClaimsPrincipal? ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}