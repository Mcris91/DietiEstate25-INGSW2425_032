using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DietiEstate.WebApi.Configs;

public class JwtConfiguration(
    string secretKey, 
    string issuer, 
    string audience,
    int accessExpiresInMinutes,
    int refreshExpiresInDays)
{
    public string SecretSecretKey { get; } = secretKey;
    public string Issuer { get; } = issuer;
    public string Audience { get; } = audience;
    public int AccessExpiresInMinutes { get; } = accessExpiresInMinutes;
    public int RefreshExpiresInDays { get; } = refreshExpiresInDays;

    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretSecretKey)),
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}