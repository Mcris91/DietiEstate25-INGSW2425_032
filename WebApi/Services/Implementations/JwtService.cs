using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DietiEstate.Shared.Constants;
using DietiEstate.Shared.Models.AuthModels;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Configs;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi.Services.Implementations;

public class JwtService(JwtConfiguration jwtConfiguration) : IJwtService
{
    public string GenerateJwtIdToken(User user)
    {
        return GenerateJwtToken(user, JwtTokenType.Id, jwtConfiguration.IdExpiresInMinutes);
    }
    
    public string GenerateJwtAccessToken(User user)
    {
        return GenerateJwtToken(user, JwtTokenType.Access, jwtConfiguration.AccessExpiresInMinutes);
    }
    public string GenerateJwtRefreshToken(User user)
    {
        return GenerateJwtToken(user, JwtTokenType.Refresh, jwtConfiguration.RefreshExpiresInDays * 24 * 60);
    }
    private string GenerateJwtToken(User user, JwtTokenType tokenType, int tokenExpiryInMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtConfiguration.SecretSecretKey);
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (ClaimTypes.Role, user.Role.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new ("token_type", tokenType.ToString().ToLower())
        };

        switch (tokenType)
        {
            case JwtTokenType.Id:
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName));
                claims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName));
                break;
            case JwtTokenType.Access:
                claims.AddRange(GetUserScopes(user).Select(scope => new Claim("scope", scope)));
                break;
            case JwtTokenType.Refresh:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null);
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes),
            Issuer = jwtConfiguration.Issuer,
            Audience = jwtConfiguration.Audience,
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
        
        return (principal.FindFirst("token_type")?.Value)!.Equals(nameof(JwtTokenType.Access), StringComparison.CurrentCultureIgnoreCase) 
            ? principal 
            : null;
    }
    public ClaimsPrincipal? ValidateJwtRefreshToken(string token)
    {
        var principal = ValidateJwtToken(token);
        if (principal is null) return null;
        
        return (principal.FindFirst("token_type")?.Value)!.Equals(nameof(JwtTokenType.Refresh), StringComparison.CurrentCultureIgnoreCase) 
            ? principal 
            : null;
    }
    private ClaimsPrincipal? ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(
                token, jwtConfiguration.GetTokenValidationParameters(), 
                out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private static List<string> GetUserScopes(User user)
    {
        var scopes = new List<string>();
        switch (user.Role)
        {
            case UserRole.SuperAdmin:
                scopes.AddRange(UserScope.SupportAdmin.All);
                break;
            case UserRole.Admin:
                scopes.AddRange(UserScope.Agent.All);
                break;
            case UserRole.Agent:
                scopes.AddRange(UserScope.Listing.All);
                break;
            case UserRole.Client:
                scopes.AddRange(UserScope.ReadListing);
                break;
            default:
                throw new ArgumentOutOfRangeException(user.Role.ToString());
        }
        return scopes;
    }
}