using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DietiEstate.Shared.Models.AuthModels;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi.Services.Implementations;

public class JwtService : IJwtService
{
    private readonly string _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "secret";
    private readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "issuer";
    private readonly string _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "audience";
    private readonly int _accessTokenExpiryInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_MINUTES_EXPIRY") ?? "15");
    private readonly int _refreshTokenExpiryInDays = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_DAYS_EXPIRY") ?? "30");
    
    public string GenerateJwtAccessToken(string userId)
    {
        return GenerateJwtToken(userId, JwtTokenType.Access, _accessTokenExpiryInMinutes);
    }
    public string GenerateJwtRefreshToken(string userId)
    {
        return GenerateJwtToken(userId, JwtTokenType.Refresh, _refreshTokenExpiryInDays * 24 * 60);
    }
    private string GenerateJwtToken(string userId, JwtTokenType tokenType, int tokenExpiryInMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);
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
            Issuer = _issuer,
            Audience = _audience,
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
    private ClaimsPrincipal? ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
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